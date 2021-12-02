using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Component
{
    public abstract class FTSBoneComponent : FTSBindingComponent
    {
        [SerializeField, Header("Axis Setting")]
        protected JointAxis Axis;

        protected float get_value(Vector3 vector)
        {
            return get_value(Axis, vector);
        }

        protected Vector3 set_value(Vector3 vector, float value)
        {
            return set_value(Axis, vector, value);
        }

        protected float dir_of_rot
        {
            get { return dir_of_rotation(Axis); }
        }
    }
}
