using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Firefly
{
    public class PauseButton : MonoBehaviour
    {
        //[SerializeField] private Sprite _pauseSprite;
        //[SerializeField] private Sprite _resumeSprite;
        //[SerializeField] private Image _icon;
        [SerializeField] private PauseMenu _pauseMenu;

        private bool _paused;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleButtonClick);
            //_icon = GetComponent<Image>();

            // close pause menu
            if (_pauseMenu.gameObject.activeSelf)
            {
                _pauseMenu.TogglePanel();
            }
        }

        private void HandleButtonClick()
        {
            _pauseMenu.TogglePanel();
            //_paused = !_paused;
            //Time.timeScale = _paused ? 0 : 1;
            //_icon.sprite = _paused ? _resumeSprite : _pauseSprite;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}