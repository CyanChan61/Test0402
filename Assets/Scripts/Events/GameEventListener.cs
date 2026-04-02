using System;
using UnityEngine;
using UnityEngine.Events;

namespace RogueCard.Events
{
    /// <summary>
    /// MonoBehaviour that subscribes to a GameEventSO and forwards it to a UnityEvent.
    /// Attach to UI objects or gameplay objects that need to respond to global events.
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEventSO gameEvent;
        [SerializeField] private UnityEvent<object> response;

        private void OnEnable()
        {
            gameEvent?.AddListener(OnEventRaised);
        }

        private void OnDisable()
        {
            gameEvent?.RemoveListener(OnEventRaised);
        }

        private void OnEventRaised(object data)
        {
            response?.Invoke(data);
        }
    }
}
