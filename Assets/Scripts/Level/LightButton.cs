using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Firefly
{
    [Serializable]
    public class LightButtonEvent : UnityEvent<Lightable.LightState> { }

    public class LightButton : Lightable
    {
        public LightButtonEvent OnButtonChange = new LightButtonEvent();
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        protected override void LightStateChange(LightState newState)
        {
            OnButtonChange.Invoke(newState);

            _anim.SetBool("burn", newState == LightState.HasLight);
        }
    }
}
