using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firefly.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Firefly
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelClearText;
        [SerializeField] private CanvasGroup _mapUIGroup;
        [SerializeField] private CanvasGroup _spawnUIGroup;

        [SerializeField] private TextMeshProUGUI _infoText;

        private bool _mapUIEnable = false;

        [SerializeField, ReadOnly]
        private List<Nest> _activatedNests = new List<Nest>();
        private int _totalNestCount;

        private void Awake()
        {
            //_levelClearText.color = _levelClearText.color.SetAlpha(0);
            _mapUIGroup.alpha = 0;

            var allNests = GameObject.FindGameObjectsWithTag("Nest");
            _totalNestCount = allNests.Length;
        }

        private void OnEnable()
        {
            GameplayManager.Instance.OnLevelClear.AddListener(DisplayClearText);
            GameplayManager.Instance.OnEnterMapMode.AddListener(ToggleMenuUI);
            GameplayManager.Instance.OnExitMapMode.AddListener(ToggleMenuUI);
            GameplayManager.Instance.OnPlayerRespawn.AddListener(HideSpawnUI);

            GameplayManager.Instance.OnUpdateNest.AddListener(RecordNest);

            GameplayManager.Instance.OnDisplayInfo.AddListener(DisplayInfo);
            GameplayManager.Instance.OnHideInfo.AddListener(HideInfo);
        }

        private void OnDisable()
        {
            GameplayManager.Instance.OnLevelClear.RemoveListener(DisplayClearText);
            GameplayManager.Instance.OnEnterMapMode.RemoveListener(ToggleMenuUI);
            GameplayManager.Instance.OnExitMapMode.RemoveListener(ToggleMenuUI);
            GameplayManager.Instance.OnPlayerRespawn.RemoveListener(HideSpawnUI);

            GameplayManager.Instance.OnUpdateNest.RemoveListener(RecordNest);

            GameplayManager.Instance.OnDisplayInfo.RemoveListener(DisplayInfo);
            GameplayManager.Instance.OnHideInfo.RemoveListener(HideInfo);
        }

        private void HideInfo()
        {
            _infoText.DOFade(0, 1f);
        }

        private void DisplayInfo(string text)
        {
            _infoText.text = text;
            _infoText.DOFade(1, 1f);
        }

        private void RecordNest(Nest nest)
        {
            if (_activatedNests.Contains(nest)) return;
            _activatedNests.Add(nest);

            if (_activatedNests.Count < _totalNestCount)
            {
                _levelClearText.text = $"Nests found: {_activatedNests.Count}/{_totalNestCount}";

            }
            else
            {
                _levelClearText.text = "All nests found!";

            }
        }

        private void HideSpawnUI()
        {
            _spawnUIGroup.DOFade(0, 1f);
        }

        private void DisplayClearText()
        {
            //_levelClearText.DOFade(1, 1f);
            _levelClearText.text = "All nests found!";
            var anim = _levelClearText.gameObject.GetComponent<DOTweenAnimation>();
            anim.DOPlay();
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