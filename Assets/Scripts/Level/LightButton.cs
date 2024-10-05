using System;
using UnityEngine;
using UnityEngine.Events;

namespace Firefly
{
    [Serializable]
    public class LightButtonEvent : UnityEvent<LightButton.ButtonState> { }

    public class LightButton : MonoBehaviour
    {
        public enum ButtonState
        {
            HasLight,
            NoLight
        }

        private int _nearLights = 0;

        public LightButtonEvent OnButtonChange = new LightButtonEvent();

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Light"))
            {
                if (_nearLights == 0)
                {
                    OnButtonChange.Invoke(ButtonState.HasLight);
                }
                _nearLights++;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {

            if (col.CompareTag("Light"))
            {
                _nearLights--;
                if (_nearLights == 0)
                {
                    OnButtonChange.Invoke(ButtonState.NoLight);
                }
            }
        }

    }
}
