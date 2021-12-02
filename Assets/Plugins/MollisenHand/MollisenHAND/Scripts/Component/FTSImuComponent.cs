using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace FTS.Component
{
    public class FTSImuComponent : FTSGloveHand.Component
    {
        private Vector3 _priv;
        private Vector3 _next;
        private Vector3 _delta;

        public float dx { get { return _delta.x; } }
        public float dy { get { return _delta.y; } }
        public float dz { get { return _delta.z; } }

        public float x { get { return _next.x; } }
        public float y { get { return _next.y; } }
        public float z { get { return _next.z; } }

        public event UnityAction<FTSImuComponent> OnChangeValue;

        void Awake()
        {
            _priv = Vector3.zero;
            _next = Vector3.zero;
            _delta = Vector2.zero;
        }

        public override void OnChangeState(FTSGloveHand hand, HandState state)
        {
            if (_priv == Vector3.zero)
                _priv = state.Euler;

            if (_priv == state.Euler)
                return;

            _next = state.Euler;
            _delta = _priv - _next;
            _priv = _next;

            OnChangeValue?.Invoke(this);
        }
    }
}
