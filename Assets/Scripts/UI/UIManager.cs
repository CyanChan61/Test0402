using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueCard.UI
{
    /// <summary>
    /// Central UI manager. Discovers all UIPanel children on Awake,
    /// then exposes Show/Hide/Get by type.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        private readonly Dictionary<Type, UIPanel> _panels = new();

        private void Awake()
        {
            // Auto-discover all panels in children
            var panels = GetComponentsInChildren<UIPanel>(includeInactive: true);
            foreach (var panel in panels)
            {
                var type = panel.GetType();
                if (!_panels.ContainsKey(type))
                    _panels[type] = panel;
            }

            // Hide all at start
            HideAll();
        }

        public void ShowPanel<T>() where T : UIPanel
        {
            if (_panels.TryGetValue(typeof(T), out var panel))
                panel.Show();
            else
                Debug.LogWarning($"[UIManager] Panel not found: {typeof(T).Name}");
        }

        public void HidePanel<T>() where T : UIPanel
        {
            if (_panels.TryGetValue(typeof(T), out var panel))
                panel.Hide();
        }

        public T GetPanel<T>() where T : UIPanel
        {
            _panels.TryGetValue(typeof(T), out var panel);
            return panel as T;
        }

        public void HideAll()
        {
            foreach (var panel in _panels.Values)
                panel.Hide();
        }
    }
}
