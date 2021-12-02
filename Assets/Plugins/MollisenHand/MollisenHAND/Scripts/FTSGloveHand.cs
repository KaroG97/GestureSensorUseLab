using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using FTS.Device;

namespace FTS
{
    public class FTSGloveHand : MonoBehaviour
    {
        private const float SENSITIVITY_MIN = 0.03f;
        private const float SENSITIVITY_MAX = 0.1f;

        [SerializeField]
        private HandType Hand;

        [SerializeField, Range(SENSITIVITY_MIN, SENSITIVITY_MAX)]
        private float Sensitivity = 0.04f;

        public HandType HandType { get { return Hand; } }

        private float[] _buffer_min = new float[10];
        private float[] _buffer_max = new float[10];
        private float[] _joints     = new float[15];

        private float[] _press = new float[3];
        private float[] _press_min = new float[3];
        private float[] _press_max = new float[3];

        private FTSSensorData _sensor;
        private HandState _state;

        // 켈리브레이션 데이터 가지고 있음.
        // 페킷 수신하면 켈리브레이션 데이터를 이용해 정리 Joint별로 저장

        private List<Component> _components = new List<Component>();

        void Start()
        {
            _state = HandState.Create();
            _components.AddRange(GetComponents<Component>());
        }

        void Update()
        {
            var dip_weight = 2.0f/3.0f;
            var write_index = 1;
            var e = Sensitivity;

            if (_sensor != null && _sensor.IsConnected()) {
                var joints = _sensor.Get(DeviceDataType.Joint);
                if (joints != null) {
                    for (int index = 0; index < 10; ++index) {
                        var value = Mathf.Clamp(range01(joints[index], _buffer_min[index], _buffer_max[index]), -0.1f, 1.0f);

                        if (float.IsNaN(value))
                            continue;

                        // Joint의 변화량이 E보다 작은 경우 값을 변경하지 않음.
                        if (Mathf.Abs(_joints[write_index] - value) < e)
                            value = _joints[write_index];

                        // Joint에 배열을 입력함.
                        _joints[write_index++] = value;
                        if (index > 1 && index % 2 == 1)
                            _joints[write_index++] = value * dip_weight;
                    }
                    HandState.SetRawData(_state, DeviceDataType.Joint, _joints);
                }

                var press = _sensor.Get(DeviceDataType.Press);
                if (press != null) {
                    for (int index = 0; index < 3; ++index) {
                        var value = Mathf.Clamp(range01(press[index], _press_min[index], _press_max[index]), 0.0f, 1.0f);

                        if (float.IsNaN(value))
                            continue;

                        if (Mathf.Abs(_press[index] - value) < e)
                            value = _press[index];

                        _press[index] = value;
                    }
                    HandState.SetRawData(_state, DeviceDataType.Press, _press);
                }

                HandState.SetRawData(_state, DeviceDataType.Quaternion, _sensor);
                HandState.SetRawData(_state, DeviceDataType.Acceleration, _sensor);
                HandState.SetRawData(_state, DeviceDataType.Battery, _sensor);

                for (int index = 0; index < _components.Count; ++index) {
                    var component = _components[index];
                    if (component.isActiveAndEnabled)
                        component.OnChangeState(this, _state);
                }
            }
        }

        private void OnApplicationQuit()
        {
           // if (_sensor.IsConnected())
           //     _sensor.Vibrator(0);
        }

        public void Vibrator(FingerType finger_type, int power)
        {
            if (_sensor != null && _sensor.IsConnected())
            {
                _sensor.Vibrator(finger_type, power);
            }
        }

        public void SetBuffer(float[] buffer, float[] buffer_min, float[] buffer_max)
        {
            if (buffer == null || buffer.Length != 10)
                return;

            if (buffer_min == null || buffer_min.Length != 10)
                return;

            if (buffer_max == null || buffer_max.Length != 10)
                return;

            //_buffer = buffer;
            _buffer_min = buffer_min;
            _buffer_max = buffer_max;
        }

        public void SetBufferPress(float[] buffer_min, float[] buffer_max)
        {
            if (buffer_min == null || buffer_min.Length != 3)
                return;

            if (buffer_max == null || buffer_max.Length != 3)
                return;

            _press_min = buffer_min;
            _press_max = buffer_max;
        }

        public void SetDevice(FTSSensorData sensor)
        {
            _sensor = sensor;
        }

        public void SetSensitivity(float value01)
        {
            var value = Mathf.Clamp(value01, 0.0f, 1.0f);
            var range = SENSITIVITY_MAX - SENSITIVITY_MIN;

            Sensitivity = value*range + SENSITIVITY_MIN;
        }

        public int GetReceiveDeltaTime()
        {
            return FTSGloveManager.Instance.GetReceiveDeltaTime(Hand);
        }

        public void Calibration(CalibrationType type)
        {
            if (_sensor != null && _sensor.IsConnected())
                FTSGloveManager.Instance?.Calibration(Hand, type, _sensor);
        }

        public void Calibration(string enum_type)
        {
            Calibration((CalibrationType)Enum.Parse(typeof(CalibrationType), enum_type));
        }

        public bool IsConnected()
        {
            return FTSGloveManager.Instance.IsConnected(Hand);
        }

        private float range01(float value, float min_value, float max_value)
        {
            return (value - min_value)/(max_value - min_value);
        }

        IEnumerable<int> range(int start, int end, int delta = 1)
        {
            for (int n = start; n < end; n += delta)
                yield return n;
            yield break;
        }

        /// <summary>
        /// 
        /// </summary>
        public class Component : MonoBehaviour
        {
            public virtual void OnChangeState(FTSGloveHand hand, HandState state)
            {

            }

            protected IEnumerable<int> range(int start, int end, int delta = 1)
            {
                for (int n = start; n < end; n += delta)
                    yield return n;
                yield break;
            }
        }
    }

    public struct HandState
    {
        private float degree_range_min { get; set; }
        private float degree_range_max { get; set; }

        private float[] q;

        private Dictionary<DeviceDataType, float[]> rawdata { get; set; }

        public float this[int index] 
        {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Joint, out raw))
                    return raw[index];
                return 0.0f;
            }
        }

        public int Length 
        {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Joint, out raw))
                    return raw.Length;
                return 0;
            }
        }

        public float ThumbCMC   { get { return this[0]; } }
        public float ThumbMCP   { get { return this[1]; } }
        public float ThumbPIP   { get { return this[2]; } }
        public float IndexMCP   { get { return this[3]; } }
        public float IndexPIP   { get { return this[4]; } }
        public float IndexDIP   { get { return this[5]; } }
        public float MiddleMCP  { get { return this[6]; } }
        public float MiddlePIP  { get { return this[7]; } }
        public float MiddleDIP  { get { return this[8]; } }
        public float RingMCP    { get { return this[9]; } }
        public float RingPIP    { get { return this[10]; } }
        public float RingDIP    { get { return this[11]; } }
        public float PinkyMCP   { get { return this[12]; } }
        public float PinkyPIP   { get { return this[13]; } }
        public float PinkyDIP   { get { return this[14]; } }

        public float PressThumb {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Press, out raw))
                    return raw[0];
                return -1.0f;
            }
        }

        public float PressIndex {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Press, out raw))
                    return raw[1];
                return -1.0f;
            }
        }

        public float PressMiddle {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Press, out raw))
                    return raw[2];
                return -1.0f;
            }
        }

        public Vector3 Acceleration
        {
            get {
                float[] raw;
                // PC 버전에 Acceleration을 지원하지 않아 임시로 사용함.
                if (rawdata.TryGetValue(DeviceDataType.Acceleration, out raw))
                    return new Vector3(raw[0]/16384.0f, raw[1]/16384.0f, raw[2]/16384.0f);
                return Vector3.zero;
            }
        }

        public Vector3 Gyroscope 
        {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Gyroscope, out raw))
                    return new Vector3(raw[0]/32.8f, raw[1]/32.8f, raw[2]/32.8f);
                return Vector3.zero;
            }
        }

        public Vector3 Magnetic 
        {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Magnetic, out raw))
                    return new Vector3(raw[0]/16384.0f, raw[1]/16384.0f, raw[2]/16384.0f);
                return Vector3.zero;
            }
        }

        public Quaternion Rotation 
        {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Quaternion, out raw))
                    return new Quaternion(raw[0], raw[1], raw[2], raw[3]);
                return Quaternion.identity;
            }
        }

        public Vector3 Euler 
        {
            get {
                return Rotation.eulerAngles;
            }
        }

        public float Battery 
        {
            get {
                float[] raw;
                if (rawdata.TryGetValue(DeviceDataType.Battery, out raw))
                    return raw[0];
                return -1.0f;
            }
        }

        public static HandState Create()
        {
            var state = new HandState();
            state.degree_range_min = 0.0f;
            state.degree_range_max = 80.0f;
            state.rawdata = new Dictionary<DeviceDataType, float[]>();
            state.q = new float[4];

            return state;
        }

        public static void SetRawData(HandState state, DeviceDataType type, float[] senser)
        {
            if (!state.rawdata.ContainsKey(type))
                state.rawdata.Add(type, senser);
            state.rawdata[type] = senser;
        }

        public static void SetRawData(HandState state, DeviceDataType type, FTSSensorData sensor)
        {
            if (Application.platform != RuntimePlatform.Android && type == DeviceDataType.Quaternion) {
                var euler = sensor.Get(DeviceDataType.Rotation);
                var quaternion = Quaternion.Euler(euler[0], euler[1], euler[2]);

                state.q[0] = quaternion.x;
                state.q[1] = quaternion.y;
                state.q[2] = quaternion.z;
                state.q[3] = quaternion.w;

                SetRawData(state, DeviceDataType.Quaternion, state.q);
            }
            else {
                var raw = sensor.Get(type);
                if (raw != null)
                    SetRawData(state, type, raw);
            }
        }
    }
}

