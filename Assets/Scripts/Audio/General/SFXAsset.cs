using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Firefly.Audio
{
    [CreateAssetMenu(menuName = "Audio/SFX Asset")]
    public class SFXAsset : ScriptableObject
    {
        [SerializeField] private List<AudioClip> _audioClips;

        [SerializeField] private float _minVolume;
        [SerializeField] private float _maxVolume;
        [SerializeField] private float _minPitch;
        [SerializeField] private float _maxPitch;

        [field: SerializeField] public AudioMixerGroup Group { get; private set; }

        public AudioClip RandomClip => _audioClips[UnityEngine.Random.Range(0, _audioClips.Count - 1)];

        public float RandomVolumne => _minVolume == _maxVolume ? _maxVolume : UnityEngine.Random.Range(_minVolume, _maxVolume);
        public float RandomPitch => _minPitch == _maxPitch ? -1 : UnityEngine.Random.Range(_minPitch, _maxPitch);

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Generate SFX Asset", true, priority = -50)]
        public static bool CreateAudioAssetValidation(UnityEditor.MenuCommand menuCommand)
        {
            return UnityEditor.Selection.objects.Any(o => o is AudioClip);
        }


        [UnityEditor.MenuItem("Assets/Generate SFX Asset", priority = -50)]
        public static void CreateAudioAsset(UnityEditor.MenuCommand menuCommand)
        {
            // skip other objects to execute only once
            if (menuCommand.context != null && menuCommand.context != UnityEditor.Selection.activeObject)
            {
                return;
            }

            var clips = UnityEditor.Selection.objects.Select(o => o as AudioClip).Where(c => c != null);
            if (clips.Count() <= 0)
            {
                Debug.LogWarning("No audio clip selected.");
                return;
            }

            SFXAsset asset = ScriptableObject.CreateInstance<SFXAsset>();
            // assign clips
            asset._audioClips = new List<AudioClip>();
            foreach (var clip in clips)
            {
                asset._audioClips.Add(clip);
            }

            string name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Scriptable Objects/Audios/NewAudioAsset.asset");
            UnityEditor.AssetDatabase.CreateAsset(asset, name);
            UnityEditor.AssetDatabase.SaveAssets();

            UnityEditor.EditorUtility.FocusProjectWindow();

            UnityEditor.Selection.activeObject = asset;
        }
#endif
    }
}