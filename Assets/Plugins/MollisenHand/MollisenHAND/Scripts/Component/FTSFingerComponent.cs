using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Component
{
    public class FTSFingerComponent : FTSBoneComponent
    {
        [SerializeField, Header("Joint Angle Range")]
        private float JointAngleMin = 0.0f;
        [SerializeField]
        private float JointAngleMax = 90.0f;

        [SerializeField, Header("Bind Bone - Thumb")]
        private Transform BindThumbCMC;
        [SerializeField]
        private Transform BindThumbMCP;
        [SerializeField]
        private Transform BindThumbPIP;

        [SerializeField, Header("Bind Bone - Index")]
        private Transform BindIndexMCP;
        [SerializeField]
        private Transform BindIndexPIP;
        [SerializeField]
        private Transform BindIndexDIP;

        [SerializeField, Header("Bind Bone - Middle")]
        private Transform BindMiddleMCP;
        [SerializeField]
        private Transform BindMiddlePIP;
        [SerializeField]
        private Transform BindMiddleDIP;

        [SerializeField, Header("Bind Bone - Ring")]
        private Transform BindRingMCP;
        [SerializeField]
        private Transform BindRingPIP;
        [SerializeField]
        private Transform BindRingDIP;

        [SerializeField, Header("Bind Bone - Pinky")]
        private Transform BindPinkyMCP;
        [SerializeField]
        private Transform BindPinkyPIP;
        [SerializeField]
        private Transform BindPinkyDIP;

        private Transform[] _binds;
        private float[] _joint_origin;

        void Awake()
        {
            _binds = new Transform[] { BindIndexMCP, BindThumbMCP, BindThumbPIP,
                                      BindIndexMCP, BindIndexPIP, BindIndexDIP,
                                      BindMiddleMCP, BindMiddlePIP, BindMiddleDIP,
                                      BindRingMCP, BindRingPIP, BindRingDIP,
                                      BindPinkyMCP, BindPinkyPIP, BindPinkyDIP};
            //_joint_origin = (from transform in _binds select get_value(transform.localEulerAngles)).ToArray();
        }

        void Start()
        {
            
        }

        public override void OnChangeState(FTSGloveHand hand, HandState state)
        {
            var angle_range = JointAngleMax - JointAngleMin;
            var angle_min = JointAngleMin;
            
            for (int index = 0; index < state.Length; ++index) {
                var transform = _binds[index];

                if (transform == null)
                    continue;

                var euler = transform.localEulerAngles;
                var angle = (state[index]*angle_range + angle_min)*dir_of_rot;

                transform.localEulerAngles = set_value(euler, _joint_origin[index] + angle);
            }
        }
    }
}

