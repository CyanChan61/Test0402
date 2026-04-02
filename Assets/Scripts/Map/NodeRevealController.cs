using System;
using RogueCard.Events;

namespace RogueCard.Map
{
    /// <summary>
    /// Handles the reveal logic for map nodes.
    /// Fires events that UI listens to for flip animations.
    /// </summary>
    public class NodeRevealController
    {
        private readonly MapTree _tree;
        private readonly GameEventSO _onNodeRevealed;

        public NodeRevealController(MapTree tree, GameEventSO onNodeRevealed)
        {
            _tree = tree;
            _onNodeRevealed = onNodeRevealed;
        }

        /// <summary>
        /// Reveals the given node's card and fires the reveal event.
        /// Only works on Hidden nodes that are children of the current node.
        /// </summary>
        public bool TryReveal(MapNode node)
        {
            if (node == null) return false;
            if (node.State != NodeState.Hidden) return false;

            var (childA, childB) = _tree.GetCurrentChildren();
            if (node != childA && node != childB) return false;

            node.Card?.Reveal();
            node.SetState(NodeState.Revealed);
            _onNodeRevealed?.Raise(node);
            return true;
        }
    }
}
