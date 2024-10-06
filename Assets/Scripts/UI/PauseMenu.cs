using System;
using System.Collections;
using System.Collections.Generic;
using Firefly.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Firefly
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _quitButton;

        [SerializeField] private SceneAsset _levelScene;

        private void OnEnable()
        {
            _resumeButton.onClick.AddListener(HandleResume);
            _resetButton.onClick.AddListener(HandleReset);
            _quitButton.onClick.AddListener(HandleQuit);
        }

        private void OnDisable()
        {
            _resumeButton.onClick.RemoveListener(HandleResume);
            _resetButton.onClick.RemoveListener(HandleReset);
            _quitButton.onClick.RemoveListener(HandleQuit);
        }

        public void TogglePanel()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            Time.timeScale = gameObject.activeSelf ? 0 : 1;
        }

        private void HandleResume()
        {
            TogglePanel();
        }

        private void HandleReset()
        {
            Time.timeScale = 1;
            GameManager.Instance.LoadScene(_levelScene, _levelScene, null, null);
        }

        private void HandleQuit()
        {
            Application.Quit();
        }
    }
}