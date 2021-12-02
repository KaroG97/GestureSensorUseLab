using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Component
{
    public class FTSJointComponent : FTSBindingComponent
    {
        public enum Finger
        {
            Thumb   = 0,
            Index   = 1,
            Middle  = 2,
            Ring    = 3,
            Pinky   = 4,
        }

        [SerializeField]
        private Finger Target;

        [SerializeField, Header("Joint Setting - MCP")]
        private Transform BindMCP;
        [SerializeField]
        private JointAxis MCPAxis;
        [SerializeField]
        private float MCPAngleMin = 0.0f;
        [SerializeField]
        private float MCPAngleMax = 90.0f;

        [SerializeField, Header("Joint Setting - PIP")]
        private Transform BindPIP;
        [SerializeField]
        private JointAxis PIPAxis;
        [SerializeField]
        private float PIPAngleMin = 0.0f;
        [SerializeField]
        private float PIPAngleMax = 90.0f;

        [SerializeField, Header("Joint Setting - DIP")]
        private Transform BindDIP;
        [SerializeField]
        private JointAxis DIPAxis;
        [SerializeField]
        private float DIPAngleMin = 0.0f;
        [SerializeField]
        private float DIPAngleMax = 90.0f;

        private Joint[] _joints;

        // Use this for initialization
        void Start()
        {
            _joints = new Joint[] {
                new Joint(BindMCP, MCPAxis, MCPAngleMin, MCPAngleMax, get_value(MCPAxis, BindMCP.localEulerAngles)),
                new Joint(BindPIP, PIPAxis, PIPAngleMin, PIPAngleMax, get_value(PIPAxis, BindPIP.localEulerAngles)),
                new Joint(BindDIP, DIPAxis, DIPAngleMin, DIPAngleMax, get_value(DIPAxis, BindDIP.localEulerAngles)),
            };
        }

        public override void OnChangeState(FTSGloveHand hand, HandState state)
        {
            var deg_index = (int)Target*3;

            for (int index = 0; index < _joints.Length; ++index) {
                var joint = _joints[index];

                if (joint == null)
                    continue;

                var euler = joint.transform.localEulerAngles;
                var range = joint.angle_max - joint.angle_min;

                var angle = (state[deg_index + index]*range + joint.angle_min)*dir_of_rotation(joint.axis);

                joint.transform.localEulerAngles = set_value(joint.axis, euler, joint.angle_origin + angle);
            }
        }

        class Joint
        {
            public Transform transform { get; private set; }
            public JointAxis axis { get; private set; }
            public float angle_min { get; private set; }
            public float angle_max { get; private set; }
            public float angle_origin { get; private set; }

            public Joint(Transform transform, JointAxis axis, float min, float max, float origin)
            {
                this.transform = transform;
                this.axis = axis;
                this.angle_min = min;
                this.angle_max = max;
                this.angle_origin = origin;
            }
        }
    }
}
