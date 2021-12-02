using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.Events;

using FTS.Common;
using FTS.Device;

namespace FTS
{
    public enum HandType
    {
        None    = 0,
        Left    = Device.DeviceType.HandL,
        Right   = Device.DeviceType.HandR,
    }

    public class FTSGloveManager : MonoBehaviour
    {
        [Header("Blueooth Setting (Android Only)"), Tooltip("이전에 연결된 장치에 대해 자동으로 연결을 시도한다.")]
        public bool AutoConnect = true;

        //-------------------------------------------------------------------//

        private const int BufferSize = 10;

        public event UnityAction<HandType, CalibrationType> OnCalibrationBegin;
        public event UnityAction<HandType, CalibrationType> OnCalibrationEnd;

        public event UnityAction<HandType> OnConnectDevice;
        public event UnityAction<HandType> OnDisconnectDevice;
        public event UnityAction<HandType> OnDetectedDevice;

        private Dictionary<HandType, Memory<float>> _buffers = new Dictionary<HandType, Memory<float>>();
        private List<FTSGloveHand>                  _gloves = new List<FTSGloveHand>();

        private FTSPlatform         _platform;
        private FTSGloveCalibration _calibration;

        /// <summary>
        /// Mollisen VR Glove Device 탐색 및 세팅.
        /// </summary>
        void Awake()
        {
            try {
                _platform = FTSPlatform.GetPlatform();
                _calibration = GetComponent<FTSGloveCalibration>();

                FTSPlatform.Setting setting = new FTSPlatform.Setting();
                setting.AutoConnect = AutoConnect;

                if (_platform.Start(setting)) {
                    _buffers.Add(HandType.Left, new Memory<float>(new Dictionary<string, int> {
                        {"buffer", BufferSize }, {"buffer_min", BufferSize }, {"buffer_max", BufferSize },
                        {"buffer_pressmin", 3 }, {"buffer_pressmax", 3 }
                    }));
                    _buffers.Add(HandType.Right, new Memory<float>(new Dictionary<string, int> {
                        {"buffer", BufferSize }, {"buffer_min", BufferSize }, {"buffer_max", BufferSize },
                        {"buffer_pressmin", 3 }, {"buffer_pressmax", 3 }
                    }));

                    _calibration.OnCalibrationBegin += ProcessOnCalibrationBegin;
                    _calibration.OnCalibrationEnd += ProcessOnCalibrationEnd;
                    _calibration.LoadCache(BufferSize);

                    _calibration.Load(HandType.Left, CalibrationType.Min, _buffers[HandType.Left].Get("buffer_min"));
                    _calibration.Load(HandType.Left, CalibrationType.Max, _buffers[HandType.Left].Get("buffer_max"));
                    _calibration.Load(HandType.Right, CalibrationType.Min, _buffers[HandType.Right].Get("buffer_min"));
                    _calibration.Load(HandType.Right, CalibrationType.Max, _buffers[HandType.Right].Get("buffer_max"));

                    _calibration.Load(HandType.Left, CalibrationType.PressMinIndex, _buffers[HandType.Left].Get("buffer_pressmin"));
                    _calibration.Load(HandType.Left, CalibrationType.PressMaxIndex, _buffers[HandType.Left].Get("buffer_pressmax"));
                    _calibration.Load(HandType.Left, CalibrationType.PressMinMiddle, _buffers[HandType.Left].Get("buffer_pressmin"));
                    _calibration.Load(HandType.Left, CalibrationType.PressMaxMiddle, _buffers[HandType.Left].Get("buffer_pressmax"));

                    _calibration.Load(HandType.Right, CalibrationType.PressMinIndex, _buffers[HandType.Right].Get("buffer_pressmin"));
                    _calibration.Load(HandType.Right, CalibrationType.PressMaxIndex, _buffers[HandType.Right].Get("buffer_pressmax"));
                    _calibration.Load(HandType.Right, CalibrationType.PressMinMiddle, _buffers[HandType.Right].Get("buffer_pressmin"));
                    _calibration.Load(HandType.Right, CalibrationType.PressMaxMiddle, _buffers[HandType.Right].Get("buffer_pressmax"));

                    foreach (var glove in FindObjectsOfType<FTSGloveHand>())
                        //Attach(glove);

                    FTSPlatform.ChangeDeviceState += delegate (HandType type, FTSPlatform.DeviceState state) {
                        switch (state) {
                        case FTSPlatform.DeviceState.Connect: OnConnectDevice?.Invoke(type); break;
                        case FTSPlatform.DeviceState.Disconnect: OnDisconnectDevice?.Invoke(type); break;
                        case FTSPlatform.DeviceState.Detected: OnDetectedDevice?.Invoke(type); break;
                        }
                    };

                    Debug.Log($"FTS Glove Init.");
                }
            }
            catch (Exception e) {
                //Debug.LogException(e, this);
            }
        }

        void Update()
        {
            _platform?.Update();    
        }

        void OnDestroy()
        {
            _platform?.Release();
            _platform = null;
        }

        public bool IsReady
        {
            get { return true; }
        }

        public void Calibration(HandType hand, CalibrationType type, FTSSensorData sensor)
        {
            if (_buffers.ContainsKey(hand)) {
                _calibration.Calibration(hand, type, sensor);
            }   
        }

        public void SetOrientation()
        {
            if (_platform != null)
                _platform.SetOrientation();
        }

        public int GetReceiveDeltaTime(HandType hand)
        {
            return _platform.Get(hand).ReceiveDeltaTime();
        }

        public bool IsConnected(HandType hand)
        {
            return _platform.Get(hand).IsConnected();
        }

        public FTSBluetooth GetBluetooth()
        {
            if (_platform != null)
                return _platform.GetBluetooth();
            return new FTSUnknownBluetooth();
        }

        private void Attach(FTSGloveHand glove)
        {
            var hand = glove.HandType;
            var buffer = _buffers[hand].Get("buffer");
            var buffer_min = _buffers[hand].Get("buffer_min");
            var buffer_max = _buffers[hand].Get("buffer_max");

            var press_min = _buffers[hand].Get("buffer_pressmin");
            var press_max = _buffers[hand].Get("buffer_pressmax");

            glove.SetBuffer(buffer, buffer_min, buffer_max);
            glove.SetBufferPress(press_min, press_max);
            glove.SetDevice(_platform.Get(hand));

            _gloves.Add(glove);
        }

        private void Detach(FTSGloveHand glove)
        {
            _gloves.Remove(glove);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hand">캘리브레이션 작업을 시작한 손 타입.</param>
        /// <param name="type">캘리브레이션 작업에 대해 Min/Max 작업 여부 </param>
        private void ProcessOnCalibrationBegin(HandType hand, CalibrationType type)
        {
            if (_buffers.ContainsKey(hand))
                Debug.Log($"{hand}] Calibration {type} Begin.");

            OnCalibrationBegin?.Invoke(hand, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void ProcessOnCalibrationEnd(CalibrationData data)
        {
            if (_buffers.ContainsKey(data.hand)) {
                if (data.type == CalibrationType.Min || data.type == CalibrationType.Max)
                    data.Copy(_buffers[data.hand].Get($"buffer_{data.type.ToString().ToLower()}"));
                else if (data.type == CalibrationType.PressMinIndex || data.type == CalibrationType.PressMinMiddle) {
                    var buffer = _buffers[data.hand].Get($"buffer_pressmin");
                    if (data.type == CalibrationType.PressMinIndex) {
                        buffer[0] = data.Get(0);
                        buffer[1] = data.Get(1);
                    }
                    else if (data.type == CalibrationType.PressMinMiddle) {
                        buffer[0] = data.Get(0);
                        buffer[2] = data.Get(1);
                    }
                }
                else if (data.type == CalibrationType.PressMaxIndex || data.type == CalibrationType.PressMaxMiddle) {
                    var buffer = _buffers[data.hand].Get($"buffer_pressmax");
                    if (data.type == CalibrationType.PressMaxIndex) {
                        buffer[0] = data.Get(0);
                        buffer[1] = data.Get(1);
                    }
                    else if (data.type == CalibrationType.PressMaxMiddle) {
                        buffer[0] = data.Get(0);
                        buffer[2] = data.Get(1);
                    }
                }
                Debug.Log($"{data.hand}] Calibration {data.type} Finisthed [{data}]");
            }

            OnCalibrationEnd?.Invoke(data.hand, data.type);
        }

        private void OnMessage(int type, string message)
        {
            Debug.Log($"FTS.Device] {message}");
        }

        /// <summary>
        /// 유틸리티 함수.
        /// <see cref="FTSGloveManager"/> Class의 Instance를 반환한다.
        /// </summary>
        public static FTSGloveManager Instance
        {
            get { return GameObject.FindObjectOfType<FTSGloveManager>(); }
        }
    }
}

