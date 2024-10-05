using System.Collections;
using System.Collections.Generic;
using Firefly.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Firefly
{
    public class FireflyManager : MonoSingleton<FireflyManager>
    {
        public UnityEvent OnFireFlyDied;

        public override void Initialize()
        {
            OnFireFlyDied = new UnityEvent();
        }
    }
}
