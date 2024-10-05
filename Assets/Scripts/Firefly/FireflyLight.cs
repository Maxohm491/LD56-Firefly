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
        [SerializeField, MinMaxSlider(.5f, 5f, true)]
        private Vector2 _lightIntensity = new Vector2(0.95f, 1.05f);
        [SerializeField, MinMaxSlider(0f, 1f, true)]
        private Vector2 _falloffStrength = new Vector2(0.6f, 0.75f);

        [SerializeField] private float _flickerInterval;
        [SerializeField] private AnimationCurve _flickerCurve;

        [SerializeField]
        private bool _startOnAwake = true;

        private Light2D _fireflyLight;

        private void Start()
        {
            _fireflyLight = GetComponent<Light2D>();
            _fireflyLight.falloffIntensity = _falloffStrength.x;
            _fireflyLight.intensity = 0;

            // Make collider the same size as the light
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            collider.radius = _fireflyLight.pointLightOuterRadius;

            if (_startOnAwake)
            {
                StartLight();
            }
        }

        public void StartLight()
        {
            // intro animation
            DOTween.To(() => _fireflyLight.intensity, v => _fireflyLight.intensity = v, _lightIntensity.x, _flickerInterval)
                .From(0)
                .OnComplete(delegate
                {
                    // loop animation
                    DOTween.To(() => _fireflyLight.intensity, v => _fireflyLight.intensity = v, _lightIntensity.y, _flickerInterval)
                        .From(_lightIntensity.x)
                        .SetEase(_flickerCurve)
                        .SetLoops(-1, LoopType.Yoyo);
                    DOTween.To(() => _fireflyLight.falloffIntensity, v => _fireflyLight.falloffIntensity = v, _falloffStrength.y, _flickerInterval)
                        .From(_falloffStrength.x)
                        .SetEase(_flickerCurve)
                        .SetLoops(-1, LoopType.Yoyo);
                });
        }
    }
}