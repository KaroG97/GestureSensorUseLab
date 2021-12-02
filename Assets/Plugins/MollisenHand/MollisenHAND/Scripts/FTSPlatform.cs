using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Android;

using FTS.Device;

namespace FTS
{
    public abstract class FTSPlatform
    {
        public struct Setting
        {
            public bool AutoConnect;
        }

        public enum DeviceState
        {
            Detected,
            Connect,
            Disconnect,
        }

        private static FTSPlatform platform;

        public static Action<HandType, byte[]> RawPacket;
        public static Action<HandType, string> RawPacketStr;
        public static Action<HandType, DeviceState> ChangeDeviceState;

        private static FTSPlatform MakePlatform()
        {
            switch (Application.platform) {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return new FTSWindowsPlatform();
            case RuntimePlatform.Android:
                return new FTSAndroidPlatform();
            default:
                return new FTSUnknownPlatform();
            }
        }

        public static FTSPlatform GetPlatform()
        {
            if (platform == null)
                platform = MakePlatform();
            return platform;
        }

        private Dictionary<HandType, FTSSensorData> _sensors;

        public FTSPlatform()
        {

        }

        public bool Start(Setting setting)
        {
            if (Initlaize(setting)) {
                _sensors = new Dictionary<HandType, FTSSensorData>();
                _sensors.Add(HandType.Left, MakeSensor(HandType.Left));
                _sensors.Add(HandType.Right, MakeSensor(HandType.Right));

                return true;
            }
            return false;
        }

        public abstract bool Initlaize(Setting setting);
        public abstract void Release();

        public virtual FTSBluetooth GetBluetooth()
        {
            return new FTSUnknownBluetooth();
        }

        public virtual void Update()
        {

        }

        public virtual void SetOrientation()
        {

        }

        protected abstract FTSSensorData MakeSensor(HandType type);

        public FTSSensorData Get(HandType hand)
        {
            FTSSensorData sensor;
            if (_sensors.TryGetValue(hand, out sensor))
                return sensor;
            return null;
        }

        protected void InvokeRawPacket(HandType type, byte[] packet)
        {
            RawPacket?.Invoke(type, packet);
        }

        protected void InvokeRawPacketStr(HandType type, byte[] packet)
        {
            StringBuilder hex = new StringBuilder(packet.Length*3);
            for (int n = 0; n < packet.Length; ++n) {
                hex.AppendFormat("{0:x2}:", packet[n]);
            }
            hex.Remove(hex.Length - 1, 1);
            InvokeRawPacketStr(type, hex.ToString());
        }

        protected void InvokeRawPacketStr(HandType type, string packet)
        {
            RawPacketStr?.Invoke(type, packet);
        }
    }

    public abstract class FTSSensorData
    {
        public abstract float[] Get(DeviceDataType type);
        public abstract bool IsConnected();
        public abstract bool Vibrator(int power);
        public abstract bool Vibrator(FingerType type, int power);
        public abstract int ReceiveDeltaTime();

        protected abstract int GetDegreeLength();
        protected abstract float GetDegreeInfo(int index);


        public float this[int index] 
        {
            get { return GetDegreeInfo(index); }
        }

        public int Length 
        {
            get => GetDegreeLength();
        }
    }


    class FTSUnknownPlatform : FTSPlatform
    {
        public override bool Initlaize(Setting setting)
        {
            return true;
        }

        public override void Release()
        {
            
        }

        protected override FTSSensorData MakeSensor(HandType hand)
        {
            return new FTSUnknownSensorData();
        }

        class FTSUnknownSensorData : FTSSensorData
        {
            public override float[] Get(DeviceDataType type)
            {
                switch (type) {
                case DeviceDataType.Acceleration: return new float[3];
                case DeviceDataType.Quaternion: return new float[4];
                case DeviceDataType.Joint: return new float[10];
                case DeviceDataType.Battery: return new float[1];
                default:
                    return null;
                }
            }

            public override bool IsConnected()
            {
                return false;
            }

            public override bool Vibrator(int power)
            {
                return false;
            }

            public override bool Vibrator(FingerType type, int power)
            {
                return false;
            }

            public override int ReceiveDeltaTime()
            {
                return -1;
            }

            protected override float GetDegreeInfo(int index)
            {
                return 0.0f;
            }

            protected override int GetDegreeLength()
            {
                return 0;
            }
        }
    }


    class FTSWindowsPlatform : FTSPlatform
    {
        public override bool Initlaize(Setting setting)
        {
            if (FTSDeviceManager.Initlaize()) {
                FTSDeviceManager.RawPacket += delegate (FTS.Device.DeviceType type, byte[] packet) {
                    HandType hand = HandType.None;
                    switch (type) {
                    case Device.DeviceType.HandL: hand = HandType.Left; break;
                    case Device.DeviceType.HandR: hand = HandType.Right; break;
                    }

                    InvokeRawPacket(hand, packet);
                    InvokeRawPacketStr(hand, packet);
                };

                FTSDeviceManager.Connect += delegate (Device.DeviceType type, IntPtr handler) {
                    HandType hand_type = HandType.None;

                    switch (type) {
                    case Device.DeviceType.HandL: hand_type = HandType.Left; break;
                    case Device.DeviceType.HandR: hand_type = HandType.Right; break;
                    }

                    if (hand_type != HandType.None)
                        ChangeDeviceState?.Invoke(hand_type, DeviceState.Connect);
                };

                FTSDeviceManager.Disconnect += delegate (Device.DeviceType type, IntPtr handler) {
                    HandType hand_type = HandType.None;

                    switch (type) {
                    case Device.DeviceType.HandL: hand_type = HandType.Left; break;
                    case Device.DeviceType.HandR: hand_type = HandType.Right; break;
                    }

                    if (hand_type != HandType.None)
                        ChangeDeviceState?.Invoke(hand_type, DeviceState.Disconnect);
                };
                return true;
            }
            return false;
        }

        public override void Release()
        {
            FTSDeviceManager.Cleanup();
        }

        public override void SetOrientation()
        {
            FTSDeviceManager.SetOrientation();
        }

        protected override FTSSensorData MakeSensor(HandType type)
        {
            return new FTSWindowsSensorData(FTSDeviceManager.Get(Convert(type)));
        }

        private FTS.Device.DeviceType Convert(HandType type)
        {
            switch (type) {
            case HandType.Left: return FTS.Device.DeviceType.HandL;
            case HandType.Right: return FTS.Device.DeviceType.HandR;
            default:
                return FTS.Device.DeviceType.HandL;
            }
        }

        class FTSWindowsSensorData : FTSSensorData
        {
            private FTSDevice _device;

            public FTSWindowsSensorData(FTSDevice device)
            {
                _device = device;
            }

            public override float[] Get(DeviceDataType type)
            {
                var sensor = _device.Get(type);
                var sensor_data = new float[sensor.Length];

                for (int n = 0; n < sensor.Length; ++n) {
                    sensor_data[n] = sensor[n];
                }

                return sensor_data;
            }

            public override bool IsConnected()
            {
                return _device.IsConnected;
            }

            public override bool Vibrator(int power)
            {
                return _device.Vibrator(power);
            }

            public override bool Vibrator(FingerType type, int power)
            {
                return _device.Vibrator(type, power);
            }

            public override int ReceiveDeltaTime()
            {
                return _device.PacketDeltaTime();
            }

            protected override int GetDegreeLength()
            {
                return _device.Get(DeviceDataType.Joint).Length;
            }

            protected override float GetDegreeInfo(int index)
            {
                return _device.Get(DeviceDataType.Joint)[index];
            }
        }
    }

    class FTSAndroidPlatform : FTSPlatform
    {
        enum AndroidDevType
        {
            NONE            = 0x0000,
            HAND_LEFT       = 0x0101,
            HAND_RIGHT      = 0x0102,
        }

        enum AndroidDataType
        {
            NONE            = 0x0000,
            JointDegree     = 0x0001,
            Press           = 0x0002,

            // IMU
            Acceleration    = 0x0101,
            Gyro            = 0x0102,
            Magnetic        = 0x0103,
            Quaternion      = 0x0104,
            Rotation        = 0x0105,

            // Device State Data
            Battery         = 0x0201,
        }

        private AndroidAppInfo _appinfo;

        private Dictionary<AndroidDevType, FTSAndroidSensorData> _sesnors;
        private Thread _worker;
        private bool _is_running;

        private AndroidJavaObject _dev;
        private AndroidJavaObject _bluetooth;
        private FTSAndroidBuffer _buffer;

        private float[] r = new float[4];
        private float[] j = new float[10];
        private float[] b = new float[1];
        private float[] a = new float[3];
        private float[] p = new float[3];

        public override bool Initlaize(Setting setting)
        {
            _appinfo = new AndroidAppInfo(this);
            _sesnors = new Dictionary<AndroidDevType, FTSAndroidSensorData>();
            _sesnors.Add(AndroidDevType.HAND_LEFT, new FTSAndroidSensorData(AndroidDevType.HAND_LEFT, _appinfo));
            _sesnors.Add(AndroidDevType.HAND_RIGHT, new FTSAndroidSensorData(AndroidDevType.HAND_RIGHT, _appinfo));

            using (AndroidJavaClass java_class = new AndroidJavaClass("com.ftsame.mollisen.MollisenDEV")) {
                if (java_class != null) {
                    _dev = java_class.CallStatic<AndroidJavaObject>("GetDEV", _appinfo);
                    Application.quitting += delegate {
                        _dev.Call("release");
                    };
                    _buffer = new FTSAndroidBuffer(_dev);

                    _sesnors[AndroidDevType.HAND_LEFT].SetAndroidInstance(_dev);
                    _sesnors[AndroidDevType.HAND_RIGHT].SetAndroidInstance(_dev);

                    if (setting.AutoConnect)
                        GetBluetooth()?.PairDevice();

                    return true;
                }
            }

            return false;
        }

        public override void Release()
        {

        }

        public override void SetOrientation()
        {
            _dev.Call("resetIMU", (int)AndroidDevType.HAND_LEFT);
            _dev.Call("resetIMU", (int)AndroidDevType.HAND_RIGHT);
        }

        public override FTSBluetooth GetBluetooth()
        {
            lock (this) {
                if (_bluetooth != null)
                    return new FTSAndroidBluetooth(_dev, _bluetooth);
                return new FTSUnknownBluetooth();
            }
        }

        public override void Update()
        {
            var sensor_left = _sesnors[AndroidDevType.HAND_LEFT];
            var sensor_right = _sesnors[AndroidDevType.HAND_RIGHT];

            if (_buffer.Get(AndroidDevType.HAND_LEFT, AndroidDataType.Rotation, r))
                sensor_left.Set(DeviceDataType.Quaternion, ConvertEulerToQuaternion(r));

            if (_buffer.Get(AndroidDevType.HAND_LEFT, AndroidDataType.Acceleration, a))
                sensor_left.Set(DeviceDataType.Acceleration, a);

            if (_buffer.Get(AndroidDevType.HAND_LEFT, AndroidDataType.JointDegree, j))
                sensor_left.Set(DeviceDataType.Joint, j);

            if (_buffer.Get(AndroidDevType.HAND_LEFT, AndroidDataType.Press, p))
                sensor_left.Set(DeviceDataType.Press, p);
            
            if (_buffer.Get(AndroidDevType.HAND_LEFT, AndroidDataType.Battery, b))
                sensor_left.Set(DeviceDataType.Battery, b);

            if (_buffer.Get(AndroidDevType.HAND_RIGHT, AndroidDataType.Rotation, r))
                sensor_right.Set(DeviceDataType.Quaternion, ConvertEulerToQuaternion(r));

            if (_buffer.Get(AndroidDevType.HAND_RIGHT, AndroidDataType.Acceleration, a))
                sensor_right.Set(DeviceDataType.Acceleration, a);

            if (_buffer.Get(AndroidDevType.HAND_RIGHT, AndroidDataType.JointDegree, j))
                sensor_right.Set(DeviceDataType.Joint, j);

            if (_buffer.Get(AndroidDevType.HAND_RIGHT, AndroidDataType.Press, p))
                sensor_right.Set(DeviceDataType.Press, p);

            if (_buffer.Get(AndroidDevType.HAND_RIGHT, AndroidDataType.Battery, b))
                sensor_right.Set(DeviceDataType.Battery, b);
        }

        protected override FTSSensorData MakeSensor(HandType type)
        {
            FTSAndroidSensorData sensor;
            if (_sesnors.TryGetValue(Convert(type), out sensor))
                return sensor;
            return null;
        }

        private AndroidDevType Convert(HandType type)
        {
            switch (type) {
            case HandType.Left: return AndroidDevType.HAND_LEFT;
            case HandType.Right: return AndroidDevType.HAND_RIGHT;
            default:
                return AndroidDevType.NONE;
            }
        }

        private float[] ConvertEulerToQuaternion(float[] euler)
        {
            var quaternion = Quaternion.Euler(euler[0], euler[1], euler[2]);

            euler[0] = quaternion.x;
            euler[1] = quaternion.y;
            euler[2] = quaternion.z;
            euler[3] = quaternion.w;

            return euler;
        }

        private void CallbackReceivePacket(AndroidDevType devtype)
        {
            if (_sesnors.ContainsKey(devtype)) {
                _sesnors[devtype].CallbackDeltatime();
            }
        }

        class FTSAndroidBuffer
        {
            private const string METHOD_NAME = "getValue";
            private const string METHOD_TYPE = "(II)[F";

            private IntPtr _class;
            private IntPtr _instance;

            private IntPtr _method;
            private jvalue[] _paramters;

            public FTSAndroidBuffer(AndroidJavaObject instance)
            {
                _class = instance.GetRawClass();
                _instance = instance.GetRawObject();

                _method = AndroidJNI.GetMethodID(_class, METHOD_NAME, METHOD_TYPE);
                _paramters = new jvalue[] { new jvalue(), new jvalue() };
            }

            public bool Get(AndroidDevType dev_type, AndroidDataType data_type, float[] dest)
            {
                _paramters[0].i = (int)dev_type;
                _paramters[1].i = (int)data_type;

                var ret = AndroidJNI.CallObjectMethod(_instance, _method, _paramters);
                var len = AndroidJNI.GetArrayLength(ret);

                if (len > 0) {
                    for (int n = 0; n < len; ++n)
                        dest[n] = AndroidJNI.GetFloatArrayElement(ret, n);
                    return true;
                }
                
                return false;
            }
        }

        class FTSAndroidSensorData : FTSSensorData
        {
            private AndroidDevType _device_type;
            private AndroidAppInfo _info;
            private AndroidJavaObject _instance;

            private Dictionary<DeviceDataType, float[]> _data;

            private DateTime    _receive_time;
            private int         _receive_dt;

            public FTSAndroidSensorData(AndroidDevType device_type, AndroidAppInfo info)
            {
                _device_type = device_type;
                _info = info;
                _instance = null;

                _data = new Dictionary<DeviceDataType, float[]>();
                _data.Add(DeviceDataType.Quaternion, new float[4]);
                _data.Add(DeviceDataType.Joint, new float[10]);
                _data.Add(DeviceDataType.Press, new float[3]);
                _data.Add(DeviceDataType.Acceleration, new float[3]);
                _data.Add(DeviceDataType.Battery, new float[1]);

                _receive_time = DateTime.Now;
            }

            public void SetAndroidInstance(AndroidJavaObject instance)
            {
                if (instance != null)
                    _instance = instance;
            }

            public override float[] Get(DeviceDataType type)
            {
                float[] sensor;
                if (_data.TryGetValue(type, out sensor))
                    return sensor;
                return new float[0];
            }

            public override bool IsConnected()
            {
                return _info.CheckConnectDevice(_device_type);
            }

            public override bool Vibrator(int power)
            {
                if (_instance != null) {
                    _instance.Call("setVibratorAll", (int)_device_type, power);
                    return true;
                }
                return false;
            }

            public override bool Vibrator(FingerType type, int power)
            {
                if (_instance != null) {
                    int finger_index = -1;

                    switch (type) {
                    case FingerType.Thumb:  finger_index = 0; break;
                    case FingerType.Index:  finger_index = 1; break;
                    case FingerType.Middle: finger_index = 2; break;
                    case FingerType.Ring:   finger_index = 3; break;
                    case FingerType.Pinky:  finger_index = 4; break;
                    }

                    _instance.Call("setVibrator", (int)_device_type, finger_index, power);

                    return true;
                }
                return true;
            }

            public override int ReceiveDeltaTime()
            {
                return _receive_dt;
            }

            public void Set(DeviceDataType type, float[] new_data)
            {
                float[] data_array;
                if (_data.TryGetValue(type, out data_array)) {
                    if (new_data.Length != data_array.Length)
                        return;

                    for (int n = 0; n < data_array.Length; ++n)
                        data_array[n] = new_data[n];
                }
            }

            public void CallbackDeltatime()
            {
                var dt_now = DateTime.Now;
                _receive_dt = (dt_now - _receive_time).Milliseconds;
                _receive_time = dt_now;
            }

            protected override float GetDegreeInfo(int index)
            {
                float[] sensor;
                if (_data.TryGetValue(DeviceDataType.Joint, out sensor)) {
                    if (0 <= index && index < sensor.Length)
                        return sensor[index];
                }
                return 0.0f;
            }

            protected override int GetDegreeLength()
            {
                float[] sensor;
                if (_data.TryGetValue(DeviceDataType.Joint, out sensor))
                    return sensor.Length;
                return 0;
            }
        }

        class AndroidAppInfo : AndroidJavaProxy
        {
            private FTSAndroidPlatform android;
            private Dictionary<int, bool> _connection; 

            public AndroidAppInfo(FTSAndroidPlatform android) : base("com.ftsame.mollisen.MollisenDEV$ApplicationBridge")
            {
                this.android = android;
                _connection = new Dictionary<int, bool>();
            }

            public AndroidJavaObject getActivity()
            {
                using (AndroidJavaClass activity_class = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    return activity_class.GetStatic<AndroidJavaObject>("currentActivity");
                }
            }

            public object getPermissionInterface()
            {
                return new MollisenBLEPermission();
            }

            public void onDeviceDetected(int devid, string deviceInfo)
            {
                _connection.Add(devid, false);

                ChangeDeviceState?.Invoke((HandType)devid, DeviceState.Detected);

                Debug.Log($"{(AndroidDevType)devid} Detect Device: {deviceInfo}");
            }

            public void onDeviceConnected(int devid, string deviceInfo)
            {
                if (_connection.ContainsKey(devid))
                    _connection[devid] = true;

                ChangeDeviceState?.Invoke((HandType)devid, DeviceState.Connect);

                Debug.Log($"{(AndroidDevType)devid} Connected Device: {deviceInfo}");
            }

            public void onDeviceDisconnected(int devid, string deviceInfo)
            {
                if (_connection.ContainsKey(devid))
                    _connection[devid] = false;

                ChangeDeviceState?.Invoke((HandType)devid, DeviceState.Disconnect);

                Debug.Log($"{(AndroidDevType)devid} Disconnected Device: {deviceInfo}");
            }

            public void createdBluetoothModule(AndroidJavaObject bluetooth)
            {
                lock (android) {
                    android._bluetooth = bluetooth;
                }
            }

            DateTime t1 = DateTime.Now;
            public void onReceivePakcet(int devid, string packet)
            {
                HandType hand_type = HandType.None;
                switch ((AndroidDevType)devid) {
                case AndroidDevType.HAND_LEFT: hand_type = HandType.Left; break;
                case AndroidDevType.HAND_RIGHT: hand_type = HandType.Right; break;
                }
                android.InvokeRawPacketStr(hand_type, packet);
                android.CallbackReceivePacket((AndroidDevType)devid);
            }

            public bool CheckConnectDevice(AndroidDevType devType)
            {
                var connect = (int)devType;
                if (_connection.ContainsKey(connect))
                    return _connection[connect];
                return false;
            }
        }

        class MollisenBLEPermission : AndroidJavaProxy
        {
            public MollisenBLEPermission() : base("com.ftsame.mollisen.MollisenBLE$Permission")
            {

            }

            public bool checkSelfPermission(string permission)
            {
                return Permission.HasUserAuthorizedPermission(permission);
            }

            public void requestPermission(string permission)
            {
                Permission.RequestUserPermission(permission);
            }
        }

        public class FTSAndroidBluetooth : FTSBluetooth
        {
            AndroidJavaObject _system;
            AndroidJavaObject _bluetooth;

            public FTSAndroidBluetooth(AndroidJavaObject system, AndroidJavaObject bluetooth)
            {
                _system = system;
                _bluetooth = bluetooth;
            }

            public override void PairDevice()
            {
                _bluetooth?.Call("pairDevice");
            }

            public override void PairDevice(Device device)
            {
                if (device.HandType != HandType.None)
                    _bluetooth.Call("pairDevice", device.Address);
            }

            public override void UnpairDevice()
            {
                _bluetooth?.Call("unpairDevice");
            }

            public override Device GetPairDevice(HandType type)
            {
                var dev_type = AndroidDevType.NONE;
                switch (type) {
                case HandType.Left: dev_type = AndroidDevType.HAND_LEFT; break;
                case HandType.Right: dev_type = AndroidDevType.HAND_RIGHT; break;
                }

                var raw = _bluetooth.Call<string>("pairedDevice", (int)dev_type);
                var dev_data = raw.Split('|');

                return new FTSAndroidDevice(dev_data[0], dev_data[1]);
            }

            public override Device[] GetScanDevice()
            {
                var raw = _bluetooth?.Call<string>("getScanDevice");
                var record = raw.Split(';');
                var results = new List<Device>();

                for (int n = 0; n < record.Length; ++n) {
                    var column = record[n].Split('|');
                    results.Add(new FTSAndroidDevice(column[0], column[1], int.Parse(column[2])));
                }

                return results.ToArray();
            }

            public class FTSAndroidDevice : FTSBluetooth.Device
            {
                public FTSAndroidDevice(string name, string address)
                    : this(name, address, 0)
                {

                }

                public FTSAndroidDevice(string name, string address, int RSSI)
                {
                    this.Name = name;
                    this.Address = address;
                    this.RSSI = RSSI;
                }
            }
        }
    }
}

