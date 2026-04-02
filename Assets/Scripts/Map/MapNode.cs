using RogueCard.Cards;

namespace RogueCard.Map
{
    public enum NodeState
    {
        Hidden,    // Face-down, unknown
        Revealed,  // Face-up, place type visible
        Current,   // Player is here
        Visited,   // Player has been here
        Locked     // Cannot be visited (e.g. path not taken)
    }

    /// <summary>
    /// A single node in the binary tree map. Holds a place card and its current state.
    /// </summary>
    public class MapNode
    {
        public string NodeId { get; }
        public int Depth { get; }

        public MapNode Parent { get; }
        public MapNode ChildA { get; private set; }
        public MapNode ChildB { get; private set; }

        public PlaceCardInstance Card { get; private set; }
        public NodeState State { get; private set; } = NodeState.Hidden;

        public bool IsLeaf => ChildA == null && ChildB == null;
        public bool IsRoot => Parent == null;

        private static int _idCounter = 0;

        public MapNode(int depth, MapNode parent = null)
        {
            NodeId = $"node_{_idCounter++}";
            Depth = depth;
            Parent = parent;
        }

        public static void ResetIdCounter() => _idCounter = 0;

        public void SetChildren(MapNode childA, MapNode childB)
        {
            ChildA = childA;
            ChildB = childB;
        }

        public void AssignCard(PlaceCardInstance card)
        {
            Card = card;
        }

        /// <summary>
        /// Replaces this node's card with a new one.
        /// The displaced card is returned via out parameter.
        /// </summary>
        public void ReplaceCard(PlaceCardInstance newCard, out PlaceCardInstance displaced)
        {
            displaced = Card;
            Card = newCard;
            // The newly placed card inherits the node's Hidden state
            // (it hasn't been revealed in this position yet)
            State = NodeState.Hidden;
        }

        public void SetState(NodeState state)
        {
            State = state;
        }

        /// <summary>
        /// Returns the sibling node (other child of parent), or null if root.
        /// </summary>
        public MapNode GetSibling()
        {
            if (Parent == null) return null;
            return Parent.ChildA == this ? Parent.ChildB : Parent.ChildA;
        }

        public override string ToString() =>
            $"MapNode[{NodeId}, depth={Depth}, state={State}, card={Card}]";
    }
}
