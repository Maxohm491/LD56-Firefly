using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Firefly.Utils
{
    public class MonoObjectPool<T> : MonoBehaviour where T: MonoBehaviour, IPooledObject<T>
    {
        [SerializeField] protected T _pooledPrefab;
        [SerializeField] private bool _collectionCheck = true;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxSize = 500;
        [Header("Debug")]
        [SerializeField, ReadOnly] private int _totalPooledObjects;
        [SerializeField, ReadOnly] private int _activePooledObjects;
        [SerializeField, ReadOnly] private int _inactivePooledObjects;
        [SerializeField, ReadOnly] private int _maxActivePooledObjects;

        public Action<T> OnPooledObjectCreateCallback;
        public Action<T> OnPooledObjectGetCallback;
        public Action<T> OnPooledObjectReleaseCallback;
        public Action<T> OnPooledObjectDestroyCallback;

        protected ObjectPool<T> _pool;

        protected virtual void Awake()
        {
            _pool = new ObjectPool<T>(OnPooledObjectCreate, OnPooledObjectGet, OnPooledObjectRelease, OnPooledObjectDestroy,
                                      _collectionCheck, _defaultCapacity, _maxSize);
            _maxActivePooledObjects = 0;
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            _totalPooledObjects = _pool.CountAll;
            _activePooledObjects = _pool.CountActive;
            _inactivePooledObjects = _pool.CountInactive;
            _maxActivePooledObjects = Mathf.Max(_activePooledObjects, _maxActivePooledObjects);
#endif
        }

        public void SetPrefab(T prefab)
        {
            _pooledPrefab = prefab;
        }

        protected virtual T OnPooledObjectCreate()
        {
            T obj = GameObject.Instantiate(_pooledPrefab, transform);
            obj.SetPool(this);
            OnPooledObjectCreateCallback?.Invoke(obj);
            return obj;
        }

        protected virtual void OnPooledObjectGet(T pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
            OnPooledObjectGetCallback?.Invoke(pooledObject);
        }

        protected virtual void OnPooledObjectRelease(T pooledObject)
        {
            OnPooledObjectReleaseCallback?.Invoke(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }

        protected virtual void OnPooledObjectDestroy(T pooledObject)
        {
            OnPooledObjectDestroyCallback?.Invoke(pooledObject);
            Destroy(pooledObject.gameObject);
        }

        public virtual T Get(bool inSceneRoot = false)
        {
            T obj = _pool.Get();
            obj.transform.parent = inSceneRoot ? null : transform;
            return obj;
        }

        public virtual T Get(Vector3 position, Quaternion rotation, bool inSceneRoot = false)
        {
            T obj = Get(inSceneRoot);
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        public virtual void Release(T obj)
        {
            obj.transform.parent = transform;
            _pool.Release(obj);
        }

        public virtual void Release(T obj, float delay)
        {
            StartCoroutine(ReleaseAfterSeconds(obj, delay));
        }

        protected virtual IEnumerator ReleaseAfterSeconds(T obj, float delay, bool fixedUpdate=false)
        {
            if (fixedUpdate)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }

            Release(obj);
        }
    }
}
