using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueCard.Events
{
    /// <summary>
    /// ScriptableObject-based event channel for loose coupling between systems.
    /// Raise() broadcasts to all registered listeners.
    /// </summary>
    [CreateAssetMenu(menuName = "RogueCard/Events/GameEvent", fileName = "NewGameEvent")]
    public class GameEventSO : ScriptableObject
    {
        private readonly List<Action<object>> _listeners = new();

        public void Raise(object data = null)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i]?.Invoke(data);
        }

        public void AddListener(Action<object> listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void RemoveListener(Action<object> listener)
        {
            _listeners.Remove(listener);
        }

        private void OnDisable()
        {
            _listeners.Clear();
        }
    }
}
