using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Utils
{
    [ExecuteAlways]
    public class ShaderGlobalUtil : MonoBehaviour
    {
        void Update()
        {
            Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
            Shader.SetGlobalVector("_CursorPosition", Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}