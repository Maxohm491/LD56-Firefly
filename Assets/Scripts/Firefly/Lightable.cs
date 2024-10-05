using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly
{
    public abstract class Lightable : MonoBehaviour
    {
        protected abstract void LightStateChange(LightState newState);

        public enum LightState
        {
            HasLight,
            NoLight
        }
        private int _nearLights = 0;

        public void GainedLight()
        {
            if (_nearLights == 0)
            {
                LightStateChange(LightState.HasLight);
            }
            _nearLights++;
        }

        public void LostLight()
        {
            _nearLights--;
            if (_nearLights == 0)
            {
                LightStateChange(LightState.NoLight);
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Light"))
            {
                GainedLight();
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {

            if (col.CompareTag("Light"))
            {
                LostLight();
            }
        }


    }
}