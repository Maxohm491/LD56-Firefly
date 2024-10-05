using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firefly.Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Firefly
{
    public class Frog : MonoBehaviour
    {
        [SerializeField]
        private float _detectRadius;
        [SerializeField, Range(0, 90)]
        private float _detectRange = 30f;
        [SerializeField]
        private LayerMask _hitMask;

        [SerializeField]
        private float _detectDelta = 5f;

        [SerializeField]
        private float _detectInterval;
        private float _detectTimer;

        [SerializeField]
        private float _targetLightIntensity = 1f;
        private Light2D _frogLight;


        private bool _eating;

        private LineRenderer _lineRend;

        private bool InFrontOf(Vector3 position) =>
            (position - transform.position).Dot(transform.up) >= _detectRange * (position - transform.position).magnitude;

        private void Awake()
        {
            _lineRend = GetComponentInChildren<LineRenderer>();
            _lineRend.SetPosition(0, transform.position);
            _lineRend.SetPosition(1, transform.position);

            _frogLight = GetComponent<Light2D>();
            _frogLight.intensity = 0;
        }

        private void OnEnable()
        {
            GameplayManager.Instance.OnPlayerRespawn.AddListener(ResetEat);
        }

        private void OnDisable()
        {
            GameplayManager.Instance.OnPlayerRespawn.RemoveListener(ResetEat);
        }

        private void Update()
        {
            if (_detectTimer <= 0 && !_eating)
            {
                Detect();
                _detectTimer = _detectInterval;
            }
            _detectTimer -= Time.deltaTime;
        }

        private void Detect()
        {
            for (float delta = -_detectRange; delta <= _detectRange; delta += _detectDelta)
            {
                var dir = Quaternion.Euler(0, 0, delta) * transform.up;
                var hit = Physics2D.Raycast(transform.position, dir, _detectRadius, _hitMask);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent<IEatable>(out var target))
                    {
                        Eat(target);
                        break;
                    }
                }
            }
        }

        private void ResetEat()
        {
            _eating = false;
        }

        private void Eat(IEatable target)
        {
            _eating = true;
            // disable control
            target.GetCaught();
            Vector3 tongueEnd = transform.position;
            // tongue shoot animation
            DOTween.To(() => tongueEnd, v =>
            {
                tongueEnd = v;
                // local space
                _lineRend.SetPosition(1, tongueEnd);
            }, target.Transform.position, .2f)
                .OnComplete(delegate
                {
                    // tongue retrieve animation
                    DOTween.To(() => tongueEnd, v =>
                    {
                        tongueEnd = v;
                        // local space
                        _lineRend.SetPosition(1, tongueEnd);
                        target.Transform.position = tongueEnd;
                    }, transform.position, .2f)
                        .OnComplete(delegate
                        {
                            // eat on animation complete
                            target.GetEaten();
                            Light();
                        });
                });
        }

        private void Light()
        {
            if (_frogLight.intensity == 0)
            {
                DOTween.To(() => _frogLight.intensity, v => _frogLight.intensity = v, _targetLightIntensity, .2f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            for (float delta = -_detectRange; delta <= _detectRange; delta += _detectDelta)
            {
                var dir = Quaternion.Euler(0, 0, delta) * transform.up;
                Gizmos.DrawLine(transform.position, transform.position + dir * _detectRadius);
            }
        }
    }
}