using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Firefly
{
    public class FireflyLight : MonoBehaviour
    {
        [SerializeField, MinMaxSlider(.5f, 1.5f, true)]
        private Vector2 _lightIntensity = new Vector2(0.95f, 1.05f);
        [SerializeField, MinMaxSlider(0f, 1f, true)]
        private Vector2 _falloffStrength = new Vector2(0.6f, 0.75f);

        [SerializeField] private float _flickerInterval;
        [SerializeField] private AnimationCurve _flickerCurve;

        private Light2D _fireflyLight;

        private void Start()
        {
            _fireflyLight = GetComponent<Light2D>();
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