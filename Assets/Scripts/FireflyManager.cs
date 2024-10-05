using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Firefly
{
    public class FireflyManager : MonoBehaviour
    {
        public static FireflyManager Instance { get; private set; }

        public UnityEvent OnFireFlyDied;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                OnFireFlyDied = new UnityEvent();
            }
        }
    }
}
