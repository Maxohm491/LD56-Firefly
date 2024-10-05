using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Firefly
{
    public class Nest : MonoBehaviour
    {
        [Tooltip("Fireflies spawn with this position and rotation")]
        [SerializeField] private Transform _spawnPoint;

        [SerializeField]
        private bool _initiallyActivated;

        // whether the nest has been activated
        [SerializeField, ReadOnly]
        private bool _activated;

        private void Awake()
        {
            _activated = _initiallyActivated;
            // initially activated spawn points
            if (_activated)
            {
                GameplayManager.Instance.OnUpdateNest.Invoke(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _activated = true;
                GameplayManager.Instance.OnUpdateNest.Invoke(this);
            }
        }
    }
}
