using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Firefly.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Firefly
{
    public class GameplayManager : MonoSingleton<GameplayManager>
    {
        //[SerializeField]
        //private FireflyManager _fireflyManager;

        [SerializeField]
        private CinemachineVirtualCamera _mapVCam;

        #region Events
        /// <summary>
        /// Triggered when player dies.
        /// </summary>
        public UnityEvent<Vector2> OnFireFlyDied { get; private set; } = new UnityEvent<Vector2>();
        /// <summary>
        /// Triggered when a new spawn point is selected.
        /// </summary>
        public UnityEvent<Nest> OnUpdateNest { get; private set; } = new UnityEvent<Nest>();

        public UnityEvent OnEnterMapMode { get; private set; } = new UnityEvent();
        public UnityEvent OnExitMapMode { get; private set; } = new UnityEvent();
        public UnityEvent OnPlayerRespawn { get; private set; } = new UnityEvent();
        public UnityEvent OnLevelClear { get; private set; } = new UnityEvent();

        public UnityEvent<string> OnDisplayInfo { get; private set; } = new UnityEvent<string>();
        public UnityEvent OnHideInfo { get; private set; } = new UnityEvent();
        #endregion

        public override void Initialize()
        {
            //_fireflyManager.Initialize();
        }

        public void EnterMapMode()
        {
            // prevent reentrancy
            if (_mapVCam.gameObject.activeSelf) return;

            // turn on map cam
            _mapVCam.gameObject.SetActive(true);
            OnEnterMapMode.Invoke();
        }

        public void ExitMapMode()
        {
            // prevent reentrancy
            if (!_mapVCam.gameObject.activeSelf) return;

            // turn off map cam
            _mapVCam.gameObject.SetActive(false);
            OnExitMapMode.Invoke();
        }
    }
}