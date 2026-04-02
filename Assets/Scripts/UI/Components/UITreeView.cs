using System.Collections.Generic;
using UnityEngine;
using RogueCard.Map;

namespace RogueCard.UI.Components
{
    /// <summary>
    /// Renders the visible portion of the binary tree map.
    /// Spawns UINodeViews for the current node and its immediate children.
    /// </summary>
    public class UITreeView : MonoBehaviour
    {
        [SerializeField] private UINodeView nodeViewPrefab;
        [SerializeField] private Transform nodeContainer;

        private readonly List<UINodeView> _spawnedViews = new();

        public void Refresh(MapNode currentNode)
        {
            Clear();

            if (currentNode == null) return;

            // Spawn current node
            SpawnNodeView(currentNode);

            // Spawn children
            if (currentNode.ChildA != null) SpawnNodeView(currentNode.ChildA);
            if (currentNode.ChildB != null) SpawnNodeView(currentNode.ChildB);
        }

        private void SpawnNodeView(MapNode node)
        {
            if (nodeViewPrefab == null || nodeContainer == null) return;
            var view = Instantiate(nodeViewPrefab, nodeContainer);
            view.Setup(node);
            _spawnedViews.Add(view);
        }

        private void Clear()
        {
            foreach (var view in _spawnedViews)
                if (view != null) Destroy(view.gameObject);
            _spawnedViews.Clear();
        }
    }
}
