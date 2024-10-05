using System.Collections.Generic;
using Firefly.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Firefly.Audio
{
    public class AudioManager : PassiveSingleton<AudioManager>
    {
        private const float LOWPASSMAXCUTOFF = 22000f;

        [field: SerializeField] public AudioMixer DefaultAudioMixer { get; private set; }

        [SerializeField] private AudioClip _music;
        [SerializeField] private AudioSource _bgmSource;

        [SerializeField] private AudioSourcePool _sfxSourcePool;
        public AudioSourcePool SFXSourcePool => _sfxSourcePool;


        public override void Initialize()
        {
            base.Initialize();
        }

        private void Start()
        {
            _bgmSource.clip = _music;
            _bgmSource.Play();
        }

        public void PlayRandomSFX(SFXAsset asset, Transform source = null)
        {
            if (asset == null) return;

            float volume = asset.RandomVolumne;
            if (source != null)
            {
                Vector2 viewPos = Camera.main.WorldToViewportPoint(source.position);
                // decrease volume if outside screen
                if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
                {
                    volume = 1 / (viewPos - Vector2.one * 0.5f).sqrMagnitude / 4;
                }
            }

            float pitch = asset.RandomPitch;
            var audioSource = _sfxSourcePool.Get();
            audioSource.PlayClip(asset.RandomClip, volume, pitch, asset.Group);
        }
    }
}