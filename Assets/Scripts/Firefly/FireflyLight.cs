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

        [SerializeField]
        private ContactFilter2D _contactFilter;

        [SerializeField] private float _flickerInterval;
        [SerializeField] private AnimationCurve _flickerCurve;

        [SerializeField]
        private bool _startOnAwake = true;

        private Light2D _fireflyLight;
        [SerializeField]
        private float _lightTriggerRadius;

        private void Awake()
        {
            _fireflyLight = GetComponent<Light2D>();
        }

        private void Start()
        {
            _fireflyLight.falloffIntensity = _falloffStrength.x;
            _fireflyLight.intensity = 0;

            if (_startOnAwake)
            {
                StartLight();
            }
        }

        public void StartLight()
        {
            TriggerLightables();

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

        private void TriggerLightables() 
        {
            List<Collider2D> results = new();
            Physics2D.OverlapCircle(transform.position, _lightTriggerRadius, _contactFilter, results);

            foreach (Collider2D collider in results) {
                if (collider.gameObject.TryGetComponent<Lightable>(out var target)) {
                    target.GainedLight();
                }
            }
        }
    }
}