using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FTS.Device
{
    public class FTSDeviceManager
    {
        public static event Action<int, string> Message;
        public static event Action<DeviceType, IntPtr> Connect;
        public static event Action<DeviceType, IntPtr> Disconnect;
        public static event Action<DeviceType, byte[]> RawPacket;

        private static Dictionary<DeviceType, FTSDevice> sensors;
        private static List<Delegate> delegates;

        public static bool Initlaize()
        {
            sensors = new Dictionary<DeviceType, FTSDevice>();
            delegates = new List<Delegate>();
            delegates.Add(new FTSDeviceAPI.FuncMessage(ReceiveCallback));
            delegates.Add(new FTSDeviceAPI.FuncEvent(ReceiveCallbackConnect));
            delegates.Add(new FTSDeviceAPI.FuncEvent(ReceiveCallbackDisconnect));
            delegates.Add(new FTSDeviceAPI.FuncRawData(ReceiveCallbackRawData));

            FTSDeviceAPI.Callback(Marshal.GetFunctionPointerForDelegate(delegates[0]));
            FTSDeviceAPI.CallbackConnect(Marshal.GetFunctionPointerForDelegate(delegates[1]));
            FTSDeviceAPI.CallbackDisconnect(Marshal.GetFunctionPointerForDelegate(delegates[2]));
            FTSDeviceAPI.CallbackRawData(Marshal.GetFunctionPointerForDelegate(delegates[3]));

            foreach (DeviceType type in Enum.GetValues(typeof(DeviceType)))
                sensors.Add(type, new FTSDevice(type));

            return FTSDeviceAPI.Initlaize();
        }

        public static void Cleanup()
        {
            sensors.Clear();
            delegates.Clear();

            FTSDeviceAPI.Cleanup();
        }

        public static FTSDevice Get(DeviceType type)
        {
            if (sensors.TryGetValue(type, out var sensor))
                return sensor;
            return null;
        }

        public static void SetOrientation()
        {
            FTSDeviceAPI.ImuSetOrientation();
        }

        private static void ReceiveCallback(int type, string str)
        {
            Message?.Invoke(type, str);
        }

        private static void ReceiveCallbackConnect(int type, IntPtr handler)
        {
            Connect?.Invoke((DeviceType)type, handler);
        }

        private static void ReceiveCallbackDisconnect(int type, IntPtr handler)
        {
            Disconnect?.Invoke((DeviceType)type, handler);
        }

        private static void ReceiveCallbackRawData(int type, IntPtr buffer, int length)
        {
            try {
                byte[] data = new byte[length];
                Marshal.Copy(buffer, data, 0, length);
                DeviceType device_type = DeviceType.HandL;

                switch(type) {
                case 1: device_type = DeviceType.HandL; break;
                case 2: device_type = DeviceType.HandR; break;
                }

                RawPacket?.Invoke(device_type, data);
            }
            catch (Exception e) {
                Message?.Invoke(0, $"Packet Paring ERR:{length}");
            }
        }
    }
}
