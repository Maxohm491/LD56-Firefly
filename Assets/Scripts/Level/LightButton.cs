using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace Firefly
{
    [Serializable]
    public class LightButtonEvent : UnityEvent<Lightable.LightState> { }

    public class LightButton : Lightable
    {
        public LightButtonEvent OnButtonChange = new LightButtonEvent();
        private Animator _anim;

        [SerializeField] private float _targetIntensity;
        private Light2D _light;
        private TweenerCore<float, float, FloatOptions> _tween;

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
            _light = GetComponent<Light2D>();
            _light.intensity = 0f;
        }

        protected override void LightStateChange(LightState newState)
        {
            OnButtonChange.Invoke(newState);

            _anim.SetBool("burn", newState == LightState.HasLight);

            if (_tween != null && _tween.IsPlaying())
            {
                _tween.Kill();
            }
            _tween = DOTween.To(() => _light.intensity, v => _light.intensity = v, newState == LightState.HasLight ? _targetIntensity : 0, 1f);
        }
    }
}
