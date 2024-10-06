using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firefly;
using MoreMountains.Feedbacks;
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

        [field: SerializeField] public MMF_Player WallMoveFX { get; private set; }


        private DOTweenPath _dotween;

        private void Awake()
        {
            _dotween = GetComponentInChildren<DOTweenPath>();
        }
 
        public void HandleButtonEvent(Lightable.LightState state)
        {
            if (state == Lightable.LightState.HasLight)
            {
                _dotween.DOPlayForward();
                WallMoveFX.PlayFeedbacks();
            }
            else if (state == Lightable.LightState.NoLight)
            {
                if (_movementType == MovementType.Rewind)
                {
                    _dotween.DOPlayBackwards();
                    WallMoveFX.PlayFeedbacks();

                }
                else if (_movementType == MovementType.Pause)
                {
                    _dotween.DOPause();
                }

            }
        }
    }
}