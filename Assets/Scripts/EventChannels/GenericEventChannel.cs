using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Firefly.EventChannels
{
    public class GenericEventChannel<T> : ScriptableObject
    {
        private UnityAction<T> _eventCallback;

        public void RaiseEvent(T arg)
        {
            _eventCallback?.Invoke(arg);
        }

        public void AddListener(UnityAction<T> callback)
        {
            _eventCallback += callback;
        }

        public void RemoveListener(UnityAction<T> callback)
        {
            _eventCallback -= callback;
        }
    }


}