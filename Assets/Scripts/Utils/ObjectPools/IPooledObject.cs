using UnityEngine;

namespace Firefly.Utils
{
    public interface IPooledObject<T> where T: MonoBehaviour, IPooledObject<T>
    {
        public void SetPool(MonoObjectPool<T> pool);
    }
}