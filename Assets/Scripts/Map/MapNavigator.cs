using RogueCard.Cards;

namespace RogueCard.Map
{
    /// <summary>
    /// Higher-level navigation helper. Wraps MapTree to provide
    /// game-logic-level movement operations.
    /// </summary>
    public class MapNavigator
    {
        private readonly MapTree _tree;

        public MapNode CurrentNode => _tree.CurrentNode;
        public bool IsAtLeaf => _tree.IsAtLeaf();
        public bool IsAtBoss => _tree.IsAtBoss();

        public MapNavigator(MapTree tree)
        {
            _tree = tree;
        }

        /// <summary>
        /// Returns (nodeA, nodeB) — the two children to present to the player.
        /// Returns (null, null) if at a leaf.
        /// </summary>
        public (MapNode nodeA, MapNode nodeB) GetChoices()
        {
            return _tree.GetCurrentChildren();
        }

        /// <summary>
        /// Attempt to navigate to a child node.
        /// </summary>
        public bool NavigateTo(MapNode node)
        {
            return _tree.MoveToNode(node);
        }

        /// <summary>
        /// Replace a node's card with the given hand card.
        /// Returns the displaced card (goes back to hand).
        /// </summary>
        public PlaceCardInstance SwapNodeCard(MapNode node, PlaceCardInstance newCard)
        {
            return _tree.ReplaceNodeCard(node, newCard);
        }
    }
}
