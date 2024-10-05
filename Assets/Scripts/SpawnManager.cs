using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Firefly
{
    public class SpawnManager : MonoBehaviour
    {
        [Tooltip("Fireflies spawn with this position and rotation")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _fireflyPrefab;

        enum LiveState
        {
            Dead, // No fireflies are alive
            Alive // Player is currently controlling a firefly
        };

        private LiveState _liveState;

        private void OnEnable()
        {
            _liveState = LiveState.Dead;
            FireflyManager.Instance.OnFireFlyDied?.AddListener(HandleDeath);
        }

        private void HandleDeath(Vector2 _)
        {
            _liveState = LiveState.Dead;
        }

        public void HandleSpaceBar(InputAction.CallbackContext context)
        {
            if(context.performed && _liveState == LiveState.Dead)
            {
                SpawnFirefly();
            }
        }

        private void SpawnFirefly()
        {
            _liveState = LiveState.Alive;
            Instantiate(_fireflyPrefab, _spawnPoint.position, _spawnPoint.rotation);
        }

        private void OnDisable()
        {
            FireflyManager.Instance.OnFireFlyDied?.RemoveListener(HandleDeath);
        }
    }
}
