using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS
{
    public class FTSDeviceState : MonoBehaviour
    {
        DateTime _start_time;

        public FTSGloveHand Left;
        public FTSGloveHand Right;

        // Start is called before the first frame update
        void Awake()
        {
            var manager = FTSGloveManager.Instance;
            FTSPlatform.RawPacketStr += delegate (HandType type, string packet) {
                //Debug.Log($"Receive Packet: {packet}");
            };

            _start_time = DateTime.Now;

            if (manager != null) {

                manager.OnConnectDevice += OnConnectDevice;
                manager.OnDisconnectDevice += OnDisconnectDevice;
            }
        }

        void OnDestroy()
        {
            //var manager = FTSGloveManager.Instance;

            //if (manager != null) {
            //    manager.OnConnectDevice -= OnConnectDevice;
            //    manager.OnDisconnectDevice -= OnDisconnectDevice;
            //}
        }

        void Update()
        {
            //if (Left.IsConnected() && Right.IsConnected())
            //    Debug.Log($"Left: {Left.GetReceiveDeltaTime()} ms, Right: {Right.GetReceiveDeltaTime()} ms");
        }

        void OnConnectDevice(HandType type)
        {
            var connect_time = DateTime.Now;
            var dt = connect_time - _start_time;

            Debug.Log($"Connect Device: {type}. {dt.Milliseconds} ms");
        }

        void OnDisconnectDevice(HandType type)
        {
            Debug.Log($"Discoonect Device: {type}");
        }
    }
}

