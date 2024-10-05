using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Firefly
{
    public class PlayerFX : MonoBehaviour
    {
        [field:SerializeField] public MMF_Player DeathFX { get; private set; }
        [field: SerializeField] public MMF_Player SlowFX { get; private set; }
        [field: SerializeField] public MMF_Player FlyFX { get; private set; }
        [field: SerializeField] public MMF_Player ZoomOutFX { get; private set; }
        [field: SerializeField] public MMF_Player ZoomInFX { get; private set; }



        private void OnEnable()
        {
            GameplayManager.Instance.OnEnterMapMode.AddListener(HandleZoomOut);
            GameplayManager.Instance.OnExitMapMode.AddListener(HandleZoomIn);
        }

        private void OnDisable()
        {
            GameplayManager.Instance.OnEnterMapMode.RemoveListener(HandleZoomOut);
            GameplayManager.Instance.OnExitMapMode.RemoveListener(HandleZoomIn);
        }

        private void HandleZoomIn()
        {
            ZoomInFX?.PlayFeedbacks();
        }

        private void HandleZoomOut()
        {
            ZoomOutFX?.PlayFeedbacks();
        }
    }
}