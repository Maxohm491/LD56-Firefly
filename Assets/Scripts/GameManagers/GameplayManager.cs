using System;
using System.Collections;
using System.Collections.Generic;
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

        #region Events
        /// <summary>
        /// Triggered when player dies.
        /// </summary>
        public UnityEvent<Vector2> OnFireFlyDied { get; private set; } = new UnityEvent<Vector2>();
        /// <summary>
        /// Triggered when a new spawn point is selected.
        /// </summary>
        public UnityEvent<Nest> OnUpdateNest { get; private set; } = new UnityEvent<Nest>();
        #endregion

        public override void Initialize()
        {
            //_fireflyManager.Initialize();
        }
    }
}