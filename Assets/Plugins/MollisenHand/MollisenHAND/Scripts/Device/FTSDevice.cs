using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTS.Device
{
    public class FTSDevice
    {
        private IntPtr                                  _handler;
        private Dictionary<DeviceDataType, SenserData>  _sensers;

        public DeviceType type { get; private set; }

        public FTSDevice(DeviceType type)
        {
            _handler = IntPtr.Zero;
            _sensers = new Dictionary<DeviceDataType, SenserData>();

            this.type = type;

            FTSDeviceManager.Connect += OnConnect;
            FTSDeviceManager.Disconnect += OnDisconnect;
        }

        ~FTSDevice()
        {
            FTSDeviceManager.Connect -= OnConnect;
            FTSDeviceManager.Disconnect -= OnDisconnect;
        }

        public bool IsConnected 
        {
            get => _handler != IntPtr.Zero;
        }

        public SenserData Get(DeviceDataType data_type)
        {
            if (IsConnected && _sensers.TryGetValue(data_type, out var senser))
                return senser;
            return null;
        }

        public bool Vibrator(int power)
        {
            if (_handler == IntPtr.Zero)
                return false;

            FTSDeviceAPI.VibratorPower(_handler, (int)FingerType.Thumb, power);
            FTSDeviceAPI.VibratorPower(_handler, (int)FingerType.Index, power);
            FTSDeviceAPI.VibratorPower(_handler, (int)FingerType.Middle, power);
            FTSDeviceAPI.VibratorPower(_handler, (int)FingerType.Ring, power);
            FTSDeviceAPI.VibratorPower(_handler, (int)FingerType.Pinky, power);

            return true;
        }

        public int PacketDeltaTime()
        {
            if (_handler != IntPtr.Zero)
                return FTSDeviceAPI.ReceiveDeltatime(_handler);
            return -1;
        }

        public bool Vibrator(FingerType finger_type, int power)
        {
            if (_handler != IntPtr.Zero) {
                return FTSDeviceAPI.VibratorPower(_handler, (int)finger_type, power);
            }
            return false;
        }

        private void OnConnect(DeviceType type, IntPtr handler)
        {
            if (this.type == type) {
                _handler = handler;
                foreach (DeviceDataType data_type in Enum.GetValues(typeof(DeviceDataType)))
                    _sensers.Add(data_type, new SenserData(handler, data_type));
            }
        }

        private void OnDisconnect(DeviceType type, IntPtr handler)
        {
            if (this.type == type && _handler == handler) {
                _sensers.Clear();
                _handler = IntPtr.Zero;
            }
        }

        public unsafe class SenserData
        {
            private float*  _buffer;
            private int     _length;

            public SenserData(IntPtr handler, DeviceDataType data_type)
            {
                FTSDeviceAPI.GetBuffer(handler, (int)data_type, out _buffer, out _length);
            }

            ~SenserData()
            {
                _buffer = null;
                _length = -1;
            }

            public int Length 
            {
                get => _length;
            }

            public float this[int index] 
            {
                get {
                    if (_buffer != null && (-1 < index && index < _length))
                        return _buffer[index];
                    return -1.0f;
                }
            }
        }
    }
}
