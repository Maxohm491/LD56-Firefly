using System;
using UnityEngine;
using UnityEngine.Events;

namespace Firefly
{
    [Serializable]
    public class LightButtonEvent : UnityEvent<Lightable.LightState> { }

    public class LightButton : Lightable
    {
        public LightButtonEvent OnButtonChange = new LightButtonEvent();

        protected override void LightStateChange(LightState newState)
        {
            OnButtonChange.Invoke(newState);
        }
    }
}
