using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Component
{
    public class FTSFingerAAComponent : FTSBoneComponent
    {
        public enum Direction
        {
            Spread,     /**! 손바닥을 편 상태에서 AA 보정 */
            Fist,       /**! 주먹 쥔 상태에서 AA 보정 */
        }

        [SerializeField]
        private Direction Mode;

        [SerializeField, Header("Bind Bone - AA")]
        private Transform BindThumb = null;
        [SerializeField]
        private Transform BindIndex;
        [SerializeField]
        private Transform BindMiddle;
        [SerializeField]
        private Transform BindRing;
        [SerializeField]
        private Transform BindPinky;

        [SerializeField, Range(-20.0f, 20.0f), Header("Angle Setting")]
        private float AngleThumb;
        [SerializeField, Range(-20.0f, 20.0f)]
        private float AngleIndex;
        [SerializeField, Range(-20.0f, 20.0f)]
        private float AngleMiddle;
        [SerializeField, Range(-20.0f, 20.0f)]
        private float AngleRing;
        [SerializeField, Range(-20.0f, 20.0f)]
        private float AnglePinky;

        private Transform[] _angle_bind_aa;
        private float[] _angle_aa;
        private float[] _angle_aa_origin;

        // Use this for initialization
        void Start()
        {
            _angle_bind_aa = new Transform[] { BindThumb, BindIndex, BindMiddle, BindRing, BindPinky };
            _angle_aa = new float[] { AngleThumb, AngleIndex, AngleMiddle, AngleRing, AnglePinky };
            _angle_aa_origin = (from transform in _angle_bind_aa select get_value(transform.localEulerAngles)).ToArray();
        }

        void OnDisable()
        {
            foreach (var index in range(0, _angle_bind_aa.Length))
                _angle_bind_aa[index].localEulerAngles = set_value(_angle_bind_aa[index].localEulerAngles, _angle_aa_origin[index]);
        }

        public override void OnChangeState(FTSGloveHand hand, HandState state)
        {
            for (int index = 0; index < _angle_bind_aa.Length; ++index) {
                var transform = _angle_bind_aa[index];

                if (transform == null)
                    continue;

                var angle = (direction - state[index*3])*_angle_aa[index];
                transform.localEulerAngles = set_value(transform.localEulerAngles, _angle_aa_origin[index] + angle);
            }
        }

        private float direction
        {
            get {
                switch (Mode) {
                    case Direction.Spread: return 1.0f;
                    case Direction.Fist:
                    default:
                        return 0.0f;
                }
            }
        }
    }
}

