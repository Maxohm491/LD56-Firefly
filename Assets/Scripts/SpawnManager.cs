using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Firefly {
    public class SpawnManager : MonoBehaviour
    {
        [Tooltip("Fireflies spawn with this position and rotation")]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _fireflyPrefab;

        // Start is called before the first frame update
        void Start()
        {
            FireflyManager.Instance.OnFireFlyDied?.AddListener(SpawnFirefly);
            
            // TODO: should not die at the beginning
            //       instead directly spawn one
            // Temp
            FireflyManager.Instance.OnFireFlyDied?.Invoke(transform.position);
        }

        void SpawnFirefly(Vector2 _) {
            Instantiate(_fireflyPrefab, _spawnPoint.position, _spawnPoint.rotation);
        }

        void OnDestroy() {
            FireflyManager.Instance.OnFireFlyDied?.RemoveListener(SpawnFirefly);
        }
    }
}
