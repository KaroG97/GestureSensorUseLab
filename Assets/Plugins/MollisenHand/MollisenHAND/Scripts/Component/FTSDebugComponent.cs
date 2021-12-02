using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace FTS.Component
{
    public class FTSDebugComponent : FTSGloveHand.Component
    {
        [SerializeField]
        private Text DegreeInfo;
        [SerializeField]
        private Text QuaternionInfo;
        [SerializeField]
        private Text VelocityInfo;
        [SerializeField]
        private Text BatteryInfo;

        public override void OnChangeState(FTSGloveHand hand, HandState state)
        {
            if (DegreeInfo != null) {
                string debug_text = "";
                for (int n = 0; n < state.Length; ++n) {
                    var div = (n + 1 != state.Length) ? " | " : "";
                    debug_text += $"{state[n],4}{div}";
                }

                DegreeInfo.text = debug_text;
            }
        }

        /*
        public override void OnQuaternion(Quaternion quaternion)
        {
            if (QuaternionInfo != null) {
                var euler = quaternion.eulerAngles;
                QuaternionInfo.text = $"X:{euler.x}, Y:{euler.y}, Z:{euler.z}";
            }
        }

        public override void OnVelocity(Vector3 velocity)
        {
            if (VelocityInfo != null) {
                VelocityInfo.text = $"X:{velocity.x}, Y:{velocity.y}, Z:{velocity.z}";
            }
        }

        public override void OnBattery(float battery)
        {
            if (BatteryInfo != null) {
                BatteryInfo.text = $"{battery}";
            }
        }
        */
    }
}

