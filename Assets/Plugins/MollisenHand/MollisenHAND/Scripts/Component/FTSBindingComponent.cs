using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Component
{
    public enum JointAxis
    {
        Forward,        // + Z-Axis
        Right,          // + X-Axis
        Up,             // + Y-Axis
        ReverseForward, // - Z-Axis
        ReverseRight,   // - X-Axis
        ReverseUp,      // - Y-Axis
    }

    public abstract class FTSBindingComponent : FTSGloveHand.Component
    {
        protected Vector3 make_vecter(JointAxis axis)
        {
            switch (axis)
            {
                case JointAxis.Forward:
                case JointAxis.ReverseForward:
                    return Vector3.forward;
                case JointAxis.Right:
                case JointAxis.ReverseRight:
                    return Vector3.right;
                case JointAxis.Up:
                case JointAxis.ReverseUp:
                    return Vector3.up;
                default:
                    return Vector3.zero;
            }
        }

        protected Vector3 make_mask(JointAxis axis)
        {
            switch (axis)
            {
                case JointAxis.Forward:
                case JointAxis.ReverseForward:
                    return new Vector3(1.0f, 1.0f, 0.0f);
                case JointAxis.Right:
                case JointAxis.ReverseRight:
                    return new Vector3(0.0f, 1.0f, 1.0f);
                case JointAxis.Up:
                case JointAxis.ReverseUp:
                    return new Vector3(1.0f, 0.0f, 1.0f);
                default:
                    return Vector3.one;
            }
        }

        protected float dir_of_rotation(JointAxis axis)
        {
            switch (axis)
            {
                case JointAxis.ReverseForward:
                case JointAxis.ReverseRight:
                case JointAxis.ReverseUp:
                    return -1.0f;
                default:
                    return 1.0f;
            }
        }

        protected float get_value(JointAxis axis, Vector3 value)
        {
            return Vector3.Scale(make_vecter(axis), value).magnitude;
        }

        protected Vector3 set_value(JointAxis axis, Vector3 vector, float value)
        {
            var other = Vector3.Scale(make_mask(axis), vector);
            var select = make_vecter(axis) * value;

            return other + select;
        }
    }
}
