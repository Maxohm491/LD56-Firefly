using System.Collections;
using System.Collections.Generic;
using Firefly.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Firefly
{
    public class FireflyBodyManager : MonoBehaviour
    {
        [SerializeField] private FireflyBody _bodyPrefab;

        private List<FireflyBody> _spawnedBodies = new List<FireflyBody>();

        private void OnEnable()
        {
            GameplayManager.Instance.OnFireFlyDied.AddListener(HandleSpawnBody);
        }

        private void OnDisable()
        {
            GameplayManager.Instance.OnFireFlyDied.RemoveListener(HandleSpawnBody);
        }

        [Button(Expanded = true)]
        private void HandleSpawnBody(Vector2 position)
        {
            var body = GameObject.Instantiate(_bodyPrefab, position, Quaternion.identity);
            _spawnedBodies.Add(body);
        }
    }
}
