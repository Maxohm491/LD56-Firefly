
using UnityEngine.Events;
using UnityEngine;

namespace Firefly.EventChannels
{
    [CreateAssetMenu(menuName = "EventChannels/VoidEventChannel")]
    public class VoidEventChannel : ScriptableObject
    {
        private UnityAction _eventCallback;

        public void RaiseEvent()
        {
            _eventCallback?.Invoke();
        }

        public void AddListener(UnityAction callback)
        {
            _eventCallback += callback;
        }

        public void RemoveListener(UnityAction callback)
        {
            _eventCallback -= callback;
        }
    }
}