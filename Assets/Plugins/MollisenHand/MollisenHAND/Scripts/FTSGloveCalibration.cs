using FTS.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FTS
{
    public enum CalibrationType
    {
        // Hand 
        Min,
        Max,
        Auto,

        // Press
        PressMin,
        PressMinIndex,
        PressMaxIndex,
        PressMinMiddle,
        PressMaxMiddle,
    }

    [Serializable]
    public class CalibrationData
    {
        [SerializeField]
        public HandType hand;
        [SerializeField]
        public CalibrationType type;
        [SerializeField]
        private float[] data;

        public CalibrationData(HandType hand, CalibrationType type, float[] data)
        {
            this.hand = hand;
            this.type = type;
            this.data = data;
        }

        public int Length
        {
            get { return data.Length; }
        }

        public float Get(int index)
        {
            if (0 <= index && index < data.Length)
                return data[index];
            return -1.0f;
        }

        public void Copy(float[] buffer)
        {
            foreach (var index in Enumerable.Range(0, Mathf.Min(data.Length, buffer.Length)))
                buffer[index] = data[index];
        }

        public override string ToString()
        {
            return $"{hand}/{type} [{string.Join(", ", from value in data select value.ToString())}]";
        }
    }

    class FTSGloveCalibration : MonoBehaviour
    {
        public event UnityAction<HandType, CalibrationType> OnCalibrationBegin;
        public event UnityAction<CalibrationData>       OnCalibrationEnd;

        private CalibrationDatabase _database = new CalibrationDatabase();

        void Awake()
        {
            OnCalibrationEnd += OnStoreCalibrationData;
        }

        public string CachePath
        {
            get { return Path.Combine(Application.persistentDataPath, "Cache"); }
        }

        public string CacheFile
        {
            get { return Path.Combine(CachePath, "CalibrationDatabase.json"); }
        }

        public void LoadCache(int buffer_size)
        {
            var asset = Resources.Load<TextAsset>("FTS.Data/CalibrationDatabase");
            if (asset != null) {
                _database = JsonUtility.FromJson<CalibrationDatabase>(asset.text);
                if (_database == null)
                    _database = new CalibrationDatabase();
                Resources.UnloadAsset(asset);
            }

            if (File.Exists(CacheFile)) {
                using (var sr = new StreamReader(File.Open(CacheFile, FileMode.Open, FileAccess.Read))) {
                    var load = JsonUtility.FromJson<CalibrationDatabase>(sr.ReadToEnd());
                    if (load != null)
                        _database = load;
                }
            }

            if (_database.size != buffer_size)
                _database = new CalibrationDatabase(buffer_size);
        }

        public void Load(HandType hand, CalibrationType type, float[] buffer)
        {
            CalibrationData calibration;
            if (_database.load(out calibration, hand, type)) {
                if (type == CalibrationType.Min || type == CalibrationType.Max)
                    calibration.Copy(buffer);
                else if (type == CalibrationType.PressMinIndex || type == CalibrationType.PressMaxIndex) {
                    buffer[0] = calibration.Get(0);
                    buffer[1] = calibration.Get(1);
                }
                else if (type == CalibrationType.PressMinMiddle || type == CalibrationType.PressMaxMiddle) {
                    buffer[0] = calibration.Get(0);
                    buffer[2] = calibration.Get(1);
                }
            }
        }

        public void Calibration(HandType hand, CalibrationType type, FTSSensorData sensor)
        {
            switch (type) {
            case CalibrationType.Max:
            case CalibrationType.Min:
                StartCoroutine(CalibrationWorker(hand, type, sensor));
                break;
            case CalibrationType.Auto:
                StartCoroutine(CalibrationAutoWorker(hand, sensor));
                break;
            case CalibrationType.PressMin:
                StartCoroutine(CalibrationPressMinWorker(hand, type, sensor));
                break;
            case CalibrationType.PressMaxIndex:
            case CalibrationType.PressMinIndex:
                StartCoroutine(CalibrationPressIndexWorker(hand, type, sensor));
                break;
            case CalibrationType.PressMaxMiddle:
            case CalibrationType.PressMinMiddle:
                StartCoroutine(CalibrationPressMiddleWorker(hand, type, sensor));
                break;
            }
        }

        private void OnStoreCalibrationData(CalibrationData calibration)
        {
            _database.store(calibration);

            if (!Directory.Exists(CachePath))
                Directory.CreateDirectory(CachePath);

            using (var sw = new StreamWriter(File.Open(CacheFile, FileMode.Create, FileAccess.Write))) {
                sw.Write(JsonUtility.ToJson(_database, true));
                sw.Flush();
            }
        }

        IEnumerator CalibrationWorker(HandType hand, CalibrationType type, FTSSensorData sensor)
        {
            int write_count = 0;
            float max_check_time = 1.0f;
            float now_check_time = 0.0f;

            float[] read_buffer = new float[sensor.Length];

            OnCalibrationBegin?.Invoke(hand, type);

            while (now_check_time < max_check_time) {
                foreach (var index in Enumerable.Range(0, sensor.Length))
                    read_buffer[index] += sensor[index];
                write_count += 1;
                now_check_time += Mathf.Min(Time.deltaTime, 0.1f);
                yield return null;
            }

            foreach (var index in Enumerable.Range(0, read_buffer.Length))
                read_buffer[index] = read_buffer[index]/write_count;

            OnCalibrationEnd?.Invoke(new CalibrationData(hand, type, read_buffer));
            yield break;
        }

        IEnumerator CalibrationAutoWorker(HandType hand, FTSSensorData sensor)
        {
            var dt = 0.1f;
            var rawdata = new RawData(sensor);
            var update = StartCoroutine(GetRawData(rawdata));
            OnCalibrationBegin?.Invoke(hand, CalibrationType.Auto);

            var now_time = 0.0f;
            var max_time = 3.0f;
            var min_value = rawdata.raw;
            var max_value = rawdata.raw;

            while (now_time < max_time) {
                float[] now_value = rawdata.now;
                for(int n = 0; n < now_value.Length; ++n) {
                    min_value[n] = Mathf.Min(min_value[n], now_value[n]);
                    max_value[n] = Mathf.Max(max_value[n], now_value[n]);
                }
                rawdata.Clear();
                now_time += dt;
                yield return new WaitForSeconds(dt);
            }

            StopCoroutine(update);

            OnCalibrationEnd?.Invoke(new CalibrationData(hand, CalibrationType.Min, min_value));
            OnCalibrationEnd?.Invoke(new CalibrationData(hand, CalibrationType.Max, max_value));

            yield break;
        }

        IEnumerator CalibrationPressMinWorker(HandType hand, CalibrationType type, FTSSensorData sensor)
        {
            int write_count = 0;
            float max_check_time = 1.0f;
            float now_check_time = 0.0f;

            float[] read_index_buffer = new float[2]; // [엄지, 검지]
            float[] read_middle_buffer = new float[2]; // [엄지, 중지]

            OnCalibrationBegin?.Invoke(hand, type);

            while (now_check_time < max_check_time) {
                var raw = sensor.Get(DeviceDataType.Press);
                read_index_buffer[0] += raw[0];
                read_index_buffer[1] += raw[1];
                read_middle_buffer[0] += raw[0];
                read_middle_buffer[1] += raw[2];
                write_count += 1;
                now_check_time += Mathf.Min(Time.deltaTime, 0.1f);
                yield return null;
            }

            foreach (var index in Enumerable.Range(0, read_index_buffer.Length)) {
                read_index_buffer[index] = read_index_buffer[index]/write_count;
                read_middle_buffer[index] = read_middle_buffer[index]/write_count;
            }
            
            OnCalibrationEnd?.Invoke(new CalibrationData(hand, CalibrationType.PressMinIndex, read_index_buffer));
            OnCalibrationEnd?.Invoke(new CalibrationData(hand, CalibrationType.PressMinMiddle, read_middle_buffer));
            yield break;
        }

        IEnumerator CalibrationPressIndexWorker(HandType hand, CalibrationType type, FTSSensorData sensor)
        {
            int write_count = 0;
            float max_check_time = 1.0f;
            float now_check_time = 0.0f;

            float[] read_buffer = new float[2]; // [엄지, {검지 | 중지}]

            OnCalibrationBegin?.Invoke(hand, type);

            while (now_check_time < max_check_time) {
                var raw = sensor.Get(DeviceDataType.Press);
                read_buffer[0] += raw[0];
                read_buffer[1] += raw[1];
                write_count += 1;
                now_check_time += Mathf.Min(Time.deltaTime, 0.1f);
                yield return null;
            }

            foreach (var index in Enumerable.Range(0, read_buffer.Length))
                read_buffer[index] = read_buffer[index]/write_count;

            OnCalibrationEnd?.Invoke(new CalibrationData(hand, type, read_buffer));
            yield break;
        }

        IEnumerator CalibrationPressMiddleWorker(HandType hand, CalibrationType type, FTSSensorData sensor)
        {
            int write_count = 0;
            float max_check_time = 1.0f;
            float now_check_time = 0.0f;

            float[] read_buffer = new float[2]; // [엄지, {검지 | 중지}]

            OnCalibrationBegin?.Invoke(hand, type);

            while (now_check_time < max_check_time) {
                var raw = sensor.Get(DeviceDataType.Press);
                read_buffer[0] += raw[0];
                read_buffer[1] += raw[2];
                write_count += 1;
                now_check_time += Mathf.Min(Time.deltaTime, 0.1f);
                yield return null;
            }

            foreach (var index in Enumerable.Range(0, read_buffer.Length))
                read_buffer[index] = read_buffer[index]/write_count;

            OnCalibrationEnd?.Invoke(new CalibrationData(hand, type, read_buffer));
            yield break;
        }

        IEnumerator GetRawData(RawData rawdata)
        {
            while (true) {
                rawdata.Capture();
                yield return null;
            }
        }

        class RawData
        {
            private float[] raw_data { get; set; }
            private int read_count { get; set; }

            private FTSSensorData sensor { get; set; }

            public RawData(FTSSensorData sensor)
            {
                this.sensor = sensor;

                this.raw_data = this.raw;
                this.read_count = 1;

                
            }

            public float[] raw 
            {
                get {
                    var src = sensor.Get(DeviceDataType.Joint);
                    var dest = new float[sensor.Length];

                    for (int n = 0; n < src.Length; ++n)
                        dest[n] = src[n];

                    return dest;
                }
            }

            public float[] now 
                {
                get {
                    float[] raw = new float[this.raw_data.Length];
                    for (int n = 0; n < raw.Length; ++n) {
                        raw[n] = raw_data[n] / (float)read_count;
                    }
                    return raw;
                }
            }

            public void Capture()
            {
                var read_data = sensor.Get(DeviceDataType.Joint);
                for (int n = 0; n < read_data.Length; ++n)
                    raw_data[n] += read_data[n];
                this.read_count += 1;
            }

            public void Clear()
            {
                this.raw_data = this.raw;
                this.read_count = 1;
            }
        }

        [Serializable]
        class CalibrationDatabase
        {
            [SerializeField]
            private int buffer_size;
            [SerializeField]
            private List<CalibrationData> dataset;

            public CalibrationDatabase() : this(0)
            {

            }

            public CalibrationDatabase(int buffer_size)
            {
                this.buffer_size = buffer_size;
                this.dataset = new List<CalibrationData>();
            }

            public int size
            {
                get { return buffer_size; }
            }

            public void store(CalibrationData calibration)
            {
                // 2019.12.13: Press Calibration으로 인해 일단 제거.
                //if (calibration.Length != buffer_size)
                //    return;

                var index = dataset.FindIndex(e => e.hand == calibration.hand && e.type == calibration.type);
                if (index == -1)
                    dataset.Add(calibration);
                else
                    dataset[index] = calibration;
            }

            public bool load(out CalibrationData calibration, HandType hand, CalibrationType type)
            {
                calibration = dataset.Find(e => e.hand == hand && e.type == type);
                return calibration != null;
            }
        }
    }
}

