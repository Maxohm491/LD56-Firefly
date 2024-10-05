using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Firefly.Utils
{
    [ExecuteAlways]
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private Image _cover;
        [SerializeField] private float _fadeTime;

        public void StartTransition(Action finishCallback)
        {
            gameObject.SetActive(true);
            _cover.DOFade(1, _fadeTime)
                .SetUpdate(true)
                .OnComplete(delegate
                {
                    finishCallback?.Invoke();
                });
        }

        public void FinishTransition(Action finishCallback)
        {
            _cover.DOFade(0, _fadeTime)
                .SetUpdate(true)
                .OnComplete(delegate
                {
                    finishCallback?.Invoke();

                    gameObject.SetActive(false);
                });
        }
    }
}