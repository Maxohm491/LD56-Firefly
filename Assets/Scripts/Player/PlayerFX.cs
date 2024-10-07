using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firefly.Audio;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Firefly
{
    public class PlayerFX : MonoBehaviour
    {
        [field: SerializeField] public MMF_Player DeathFX { get; private set; }
        [field: SerializeField] public MMF_Player SlowFX { get; private set; }
        //[field: SerializeField] public MMF_Player FlyFX { get; private set; }
        //[field: SerializeField] public MMF_Player SlowFlyFX { get; private set; }
        [field: SerializeField] public MMF_Player ZoomOutFX { get; private set; }
        [field: SerializeField] public MMF_Player ZoomInFX { get; private set; }
        [field: SerializeField] public MMF_Player NestSelectFX { get; private set; }


        [SerializeField] private AudioSource _flySource;
        [SerializeField] private AudioSource _slowFlySource;

        [SerializeField] private AnimationCurve _flyVolume;
        [SerializeField] private AnimationCurve _slowFlyVolume;

        [SerializeField] private ParticleSystem _flyParticle;

        private float _flyTargetVolume;
        private float _slowFlyTargetVolume;

        private void OnEnable()
        {
            GameplayManager.Instance.OnEnterMapMode.AddListener(HandleZoomOut);
            GameplayManager.Instance.OnExitMapMode.AddListener(HandleZoomIn);
        }

        private void OnDisable()
        {
            GameplayManager.Instance.OnEnterMapMode.RemoveListener(HandleZoomOut);
            GameplayManager.Instance.OnExitMapMode.RemoveListener(HandleZoomIn);
        }

        private void Update()
        {
            _flySource.volume = _flyVolume.Evaluate(Mathf.Sin(Time.time)) * _flyTargetVolume;
            _slowFlySource.volume = _slowFlyVolume.Evaluate(Mathf.Sin(Time.time)) * _slowFlyTargetVolume;
        }

        private void HandleZoomIn()
        {
            ZoomInFX?.PlayFeedbacks();
            AudioManager.Instance.DefaultAudioMixer.DOSetFloat("AmbientVolume", 0, .5f);
        }

        private void HandleZoomOut()
        {
            ZoomOutFX?.PlayFeedbacks();
            AudioManager.Instance.DefaultAudioMixer.DOSetFloat("AmbientVolume", -10, .5f);
        }

        public void StartFly()
        {
            _flyTargetVolume = 1;
            _slowFlyTargetVolume = 0;

            _flyParticle.Play();
        }

        public void SwitchFlyMode(bool slow)
        {
            var target = Mathf.Max(_flyTargetVolume, _slowFlyTargetVolume);
            _flyTargetVolume = slow ? 0 : target;
            _slowFlyTargetVolume = slow ? target : 0;
        }

        public void StopFly()
        {
            _flyTargetVolume = 0;
            _slowFlyTargetVolume = 0;

            _flyParticle.Stop();
        }

        internal void ToggleTrail(bool lightOn)
        {
            if (lightOn)
            {
                _flyParticle.Play();
            }
            if (!lightOn)
            {
                _flyParticle.Stop();
            }
        }
    }
}