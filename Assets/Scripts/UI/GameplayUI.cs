using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firefly.Utils;
using TMPro;
using UnityEngine;

namespace Firefly
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelClearText;
        [SerializeField] private CanvasGroup _mapUIGroup;
        [SerializeField] private CanvasGroup _spawnUIGroup;

        private bool _mapUIEnable = false;

        private void Awake()
        {
            _levelClearText.color = _levelClearText.color.SetAlpha(0);
            _mapUIGroup.alpha = 0;
        }

        private void OnEnable()
        {
            GameplayManager.Instance.OnLevelClear.AddListener(DisplayClearText);
            GameplayManager.Instance.OnEnterMapMode.AddListener(ToggleMenuUI);
            GameplayManager.Instance.OnExitMapMode.AddListener(ToggleMenuUI);
            GameplayManager.Instance.OnPlayerRespawn.AddListener(HideSpawnUI);
        }

        private void OnDisable()
        {
            GameplayManager.Instance.OnLevelClear.RemoveListener(DisplayClearText);
            GameplayManager.Instance.OnEnterMapMode.RemoveListener(ToggleMenuUI);
            GameplayManager.Instance.OnExitMapMode.RemoveListener(ToggleMenuUI);
            GameplayManager.Instance.OnPlayerRespawn.RemoveListener(HideSpawnUI);
        }

        private void HideSpawnUI()
        {
            _spawnUIGroup.DOFade(0, 1f);
        }

        private void DisplayClearText()
        {
            _levelClearText.DOFade(1, 1f);
        }

        private void ToggleMenuUI()
        {
            _mapUIEnable = !_mapUIEnable;
            _mapUIGroup.DOFade(_mapUIEnable ? 1 : 0, 1f);
            if (!_mapUIEnable)
            {
                _spawnUIGroup.DOFade(1, 1f);
            }
        }
    }
}