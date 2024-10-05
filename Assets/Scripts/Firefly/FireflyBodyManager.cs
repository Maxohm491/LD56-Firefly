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

        [SerializeField] private Vector2 _gridSize = new Vector2(.5f, .5f);

        private List<Vector2Int> _occupiedPos = new List<Vector2Int>();

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
            var ipos = new Vector2Int((int)(position.x / _gridSize.x), (int)(position.y / _gridSize.y));
            if (_occupiedPos.Contains(ipos)) return;

            _occupiedPos.Add(ipos);

            var body = GameObject.Instantiate(_bodyPrefab, position, Quaternion.identity);
            _spawnedBodies.Add(body);
        }
    }
}
