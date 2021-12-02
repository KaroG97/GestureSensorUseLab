using System;
using System.Runtime.InteropServices;

namespace FTS.Device
{
    public enum DeviceType
    {
        HandL = 0x00000101,
        HandR = 0x00000102,
    }

    public enum DeviceDataType
    {
        Joint           = 0x0001,
        Press           = 0x0002,

        // IMU & AHRS Data
        Acceleration    = 0x0101,
        Gyroscope       = 0x0102,
        Magnetic        = 0x0103,
        Quaternion      = 0x0104,
        Rotation        = 0x0105,

        // Device State Data
        Battery         = 0x0201,
    }

    public enum FingerType
    {
        Thumb   = 0x01000001,
        Index   = 0x01000002,
        Middle  = 0x01000003,
        Ring    = 0x01000004,
        Pinky   = 0x01000005,
    }

    internal static class FTSDeviceAPI
    {
        public const string LinkDLL = "MollisenAPI";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FuncMessage(int type, [MarshalAs(UnmanagedType.LPWStr)] string str);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FuncEvent(int type, IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FuncRawData(int type, IntPtr buffer, int length);

        [DllImport(LinkDLL, EntryPoint = "FTSInitlaize")]
        public static extern bool Initlaize();

        [DllImport(LinkDLL, EntryPoint = "FTSCleanup")]
        public static extern void Cleanup();

        [DllImport(LinkDLL, EntryPoint = "FTSCallback")]
        public static extern void Callback(IntPtr func);

        [DllImport(LinkDLL, EntryPoint = "FTSCallbackConnect")]
        public static extern void CallbackConnect(IntPtr func);

        [DllImport(LinkDLL, EntryPoint = "FTSCallbackDisconnect")]
        public static extern void CallbackDisconnect(IntPtr func);

        [DllImport(LinkDLL, EntryPoint = "FTSCallbackRawData")]
        public static extern void CallbackRawData(IntPtr func);

        [DllImport(LinkDLL, EntryPoint = "FTSGetDeviceHandle")]
        public static extern bool GetDeviceHandle(int dev_type, out IntPtr handle);

        [DllImport(LinkDLL, EntryPoint = "FTSGetDeviceInfo")]
        public static extern bool GetDeviceInfo(IntPtr handle, out IntPtr info);

        [DllImport(LinkDLL, EntryPoint = "FTSGetBuffer")]
        public unsafe static extern bool GetBuffer(IntPtr handle, int dat_type, out float* buffer, out int lenght);

        [DllImport(LinkDLL, EntryPoint = "FTSVibratorPower")]
        public static extern bool VibratorPower(IntPtr handle, int finger_type, int power);

        [DllImport(LinkDLL, EntryPoint = "FTSVibratorStop")]
        public static extern void VibratorStop(IntPtr handle);

        [DllImport(LinkDLL, EntryPoint = "FTSImuSetOrientation")]
        public static extern void ImuSetOrientation();

        [DllImport(LinkDLL, EntryPoint = "FTSReceiveDeltatime")]
        public static extern int ReceiveDeltatime(IntPtr handle);
    }
}
