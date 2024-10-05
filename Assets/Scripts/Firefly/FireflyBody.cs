using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Firefly
{
    public class FireflyBody : MonoBehaviour
    {
        [SerializeField, MinMaxSlider(0, 2, true)]
        private Vector2 _lightIntensity;
        [SerializeField, MinMaxSlider(0, 1, true)]
        private Vector2 _falloffStrength;

        [SerializeField] private float _flickerInterval;
        [SerializeField] private AnimationCurve _flickerCurve;

        private Light2D _fireflyLight;

        private void Start()
        {
            _fireflyLight = GetComponentInChildren<Light2D>();
            DOTween.To(() => _fireflyLight.intensity, v => _fireflyLight.intensity = v, _lightIntensity.y, _flickerInterval)
                .From(_lightIntensity.x)
                .SetEase(_flickerCurve)
                .SetLoops(-1, LoopType.Yoyo);
            DOTween.To(() => _fireflyLight.falloffIntensity, v => _fireflyLight.falloffIntensity = v, _falloffStrength.y, _flickerInterval)
                .From(_falloffStrength.x)
                .SetEase(_flickerCurve)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}