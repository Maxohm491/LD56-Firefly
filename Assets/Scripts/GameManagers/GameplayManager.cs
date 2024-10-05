using System.Collections;
using System.Collections.Generic;
using Firefly.Utils;
using UnityEngine;

namespace Firefly
{
    public class GameplayManager : MonoSingleton<GameplayManager>
    {
        [SerializeField]
        private FireflyManager _fireflyManager;

        public override void Initialize()
        {
            _fireflyManager.Initialize();
        }
    }
}