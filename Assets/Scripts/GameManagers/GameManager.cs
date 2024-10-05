using System;
using System.Collections;
using System.Collections.Generic;
using Firefly.Audio;
using Firefly.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Firefly
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Header("Managers")]
        [SerializeField] private AudioManager _audioManager;

        [field: Header("References")]
        [field: SerializeField] public SceneTransition Transition { get; private set; }

        public override void Initialize()
        {
            _audioManager.Initialize();
        }

        private void Start()
        {
            // always first load main menu
#if !UNITY_EDITOR
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
#endif
        }

        public void LoadScene(SceneAsset toUnload, SceneAsset toLoad, Action loadFinishCallback, Action transitionFinsihCallback)
        {
            LoadScene(toUnload.name, toLoad, loadFinishCallback, transitionFinsihCallback);
        }

        public void LoadScene(string toUnload, SceneAsset toLoad, Action loadFinishCallback, Action transitionFinsihCallback)
        {
            Transition.StartTransition(delegate
            {
                var unload = SceneManager.UnloadSceneAsync(toUnload);
                unload.completed += delegate
                {
                    var load = SceneManager.LoadSceneAsync(toLoad.SceneName, LoadSceneMode.Additive);
                    load.completed += delegate
                    {
                        Transition.FinishTransition(transitionFinsihCallback);
                        loadFinishCallback?.Invoke();
                    };
                };
            });
        }
    }
}