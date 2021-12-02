using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Android;

namespace FTS
{
    /// <summary>
    /// Mollisen HAND III 제품의 블루투스 기능을 제어합니다.
    /// 현제 Android 환경에서만 동작합니다.
    /// </summary>
    public abstract class FTSBluetooth
    {
        public class Device
        {
            public string Name { get; protected set; }
            public string Address { get; protected set; }
            public int RSSI { get; protected set; }

            public HandType HandType {
                get {
                    switch (Name) {
                    case "FTS_VR2_L": return HandType.Left;
                    case "FTS_VR2_R": return HandType.Right;
                    default:
                        return HandType.None;
                    }
                }
            }

            public static Device Empty 
            {
                get { return new Device(); }
            }
        }

        /// <summary>
        /// 주변의 Mollisen HAND III 디바이스를 검색하고 Left, Right에 대해 자동으로 연결합니다.
        /// Mollisen HAND III 장치가 다수 켜져 있는 경우 렌덤하게 페어링 되므로 주의가 필요합니다.
        /// </summary>
        public abstract void PairDevice();

        /// <summary>
        /// 특정 Mollisen HAND III 디바이스에 대해 연결을 진행합니다.
        /// Mac Address로 연결을 진행하며 해당 정보는 Android의 경우 nRF Connect 앱으로 확인할 수 있습니다.
        /// </summary>
        /// <param name="address">해당 기기의 Mac Address</param>
        public abstract void PairDevice(Device device);

        /// <summary>
        /// 연결되어 있는 Mollisen HAND III 디바이스의 연결을 종료합니다.
        /// </summary>
        public abstract void UnpairDevice();

        /// <summary>
        /// 현재 연결되어 있는 Mollisen HAND III의 Mac Address를 반환합니다.
        /// </summary>
        /// <param name="type">연결된 기기의 타입</param>
        /// <param name="address">연결된 기기의 Mac Address</param>
        /// <returns>연결된 디바이스가 없는 경우 False를 반환합니다.</returns>
        public abstract Device GetPairDevice(HandType type);

        /// <summary>
        /// 검색된 Mollisen HAND III 디바이스 목록을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public abstract Device[] GetScanDevice();
    }

    public class FTSUnknownBluetooth : FTSBluetooth
    {
        public override void PairDevice()
        {
                       
        }

        public override void PairDevice(Device device)
        {
            
        }

        public override void UnpairDevice()
        {
            
        }

        public override Device GetPairDevice(HandType type)
        {
            return Device.Empty;
        }

        public override Device[] GetScanDevice()
        {
            return new Device[0];
        }
    }

    
}

