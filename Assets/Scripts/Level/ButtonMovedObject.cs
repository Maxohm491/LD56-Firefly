using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firefly;
using UnityEngine;

namespace Firefly
{
    public class ButtonMovedObject : MonoBehaviour
    {
        private enum MovementType
        {
            Rewind,
            Pause
        }

        [Tooltip("When it loses light what does it do")]
        [SerializeField] private MovementType _movementType = MovementType.Rewind;

        private DOTweenPath _dotween;

        private void Awake()
        {
            _dotween = GetComponentInChildren<DOTweenPath>();
        }

        public void HandleButtonEvent(LightButton.ButtonState state)
        {
            if (state == LightButton.ButtonState.HasLight)
            {
                Debug.Log("playing");
                _dotween.DOPlayForward();
            }
            else if (state == LightButton.ButtonState.NoLight)
            {
                if (_movementType == MovementType.Rewind)
                {
                    Debug.Log("rewinding");
                    // _dotween.DOFlip();
                    _dotween.DOPlayBackwards();

                }
                else if (_movementType == MovementType.Pause)
                {
                    _dotween.DOPause();
                }

            }
        }
    }
}