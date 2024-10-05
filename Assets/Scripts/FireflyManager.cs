using System.Collections;
using System.Collections.Generic;
using Firefly.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Firefly
{
    public class FireflyManager : MonoSingleton<FireflyManager>
    {
        public UnityEvent<Vector2> OnFireFlyDied;

        [SerializeField] private FireflyBody _bodyPrefab;

        public override void Initialize()
        {
            OnFireFlyDied = new UnityEvent<Vector2>();
        }

        private void OnEnable()
        {
            OnFireFlyDied.AddListener(HandleSpawnBody);
        }

        private void OnDisable()
        {
            OnFireFlyDied.RemoveListener(HandleSpawnBody);
        }

        [Button(Expanded = true)]
        private void HandleSpawnBody(Vector2 position)
        {
            GameObject.Instantiate(_bodyPrefab, position, Quaternion.identity);
        }
    }
}
