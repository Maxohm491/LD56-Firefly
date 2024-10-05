using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Firefly
{
    /// <summary>
    /// Only for closing editor light.
    /// </summary>
    public class DarkGlobalLight : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Light2D>().intensity = 0;
        }
    }
}