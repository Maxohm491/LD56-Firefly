using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Utils
{
    public class PooledParticleSystem : MonoBehaviour, IPooledObject<PooledParticleSystem>
    {
        private MonoObjectPool<PooledParticleSystem> _pool;
        public ParticleSystem Particle { get; private set; }

        // Start is called before the first frame update
        void Awake()
        {
            Particle = GetComponent<ParticleSystem>();
            var main = Particle.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void SetPool(MonoObjectPool<PooledParticleSystem> pool)
        {
            _pool = pool;
        }

        private void OnParticleSystemStopped()
        {
            if (_pool != null)
            {
                _pool.Release(this);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}

