using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firefly.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Firefly
{
    public class Nest : MonoBehaviour
    {
        [Tooltip("Fireflies spawn with this position and rotation")]
        [SerializeField] private Transform _spawnPoint;

        [SerializeField]
        private bool _initiallyActivated;

        [SerializeField]
        private SpriteRenderer _selectionIcon;

        // whether the nest has been activated
        [SerializeField, ReadOnly]
        private bool _activated;

        private FireflyLight _nestLight;

        private Animator _nestAnim;

        public bool Activated
        {
            get => _activated;
            private set
            {
                if (_activated == value) return;

                _activated = value;
                if (_activated)
                {
                    // start animation
                    _nestAnim.SetBool("activated", _activated);
                    // turn on nest light
                    _nestLight.StartLight();
                    // propgate nest info
                    GameplayManager.Instance.OnUpdateNest.Invoke(this);
                }
            }
        }

        private void Awake()
        {
            _nestLight = GetComponentInChildren<FireflyLight>();
            _nestAnim = GetComponentInChildren<Animator>();

            _selectionIcon.color = _selectionIcon.color.SetAlpha(0);
        }

        private void OnEnable()
        {
            GameplayManager.Instance.OnEnterMapMode.AddListener(HandleEnterMapMode);
            GameplayManager.Instance.OnExitMapMode.AddListener(HandleExitMapMode);
        }

        private void OnDisable()
        {
            GameplayManager.Instance.OnEnterMapMode.RemoveListener(HandleEnterMapMode);
            GameplayManager.Instance.OnExitMapMode.RemoveListener(HandleExitMapMode);
        }

        private void Start()
        {
            Activated = _initiallyActivated;
        }

        private void HandleEnterMapMode()
        {
            if (!Activated) return;

            _selectionIcon.gameObject.SetActive(true);
            _selectionIcon.DOFade(1f, 1f);
        }

        private void HandleExitMapMode()
        {
            if (!Activated) return;

            _selectionIcon.DOFade(0f, 1f)
                .OnComplete(delegate
                {
                    _selectionIcon.gameObject.SetActive(false);
                    ToggleSelection(false);
                });
        }

        public void ToggleSelection(bool enable)
        {
            _selectionIcon.material.SetFloat("_OutlineActive", enable ? 1f : 0f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Activated = true;
            }
        }
    }
}
