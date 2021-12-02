using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Component
{
    public class FTSImuRotationComponent : FTSGloveHand.Component
    {
        enum RotationMode
        {
            Eulur,
            Quaternion,
        }

        [SerializeField]
        private RotationMode Mode = RotationMode.Quaternion;
        [SerializeField]
        private FTSImuComponent IMU;

        private Vector3 _eulur;
        private Quaternion sss;


        private void Awake()
        {
            _eulur = this.transform.localRotation.eulerAngles;
            sss = Quaternion.Euler(0, 180, 0);
        }

        public override void OnChangeState(FTSGloveHand hand, HandState state)
        {
            if (Mode == RotationMode.Quaternion)
                this.transform.localRotation = sss * state.Rotation;
            else {
                var dx = (Mathf.Abs(IMU.dx) > 0.1f)?IMU.dx:0.0f;
                var dy = (Mathf.Abs(IMU.dy) > 0.1f)?IMU.dy:0.0f;
                var dz = (Mathf.Abs(IMU.dz) > 0.1f)?IMU.dz:0.0f;
                _eulur += new Vector3(dx, dy, dz);
                var rot_x = Quaternion.AngleAxis(_eulur.x, Vector3.right);
                var rot_y = Quaternion.AngleAxis(_eulur.y, Vector3.up);
                var rot_z = Quaternion.AngleAxis(_eulur.z, Vector3.forward);
                
                this.transform.localRotation = rot_x * rot_y * rot_z;//Quaternion.Euler(_eulur);
            }
        }
    }
}

