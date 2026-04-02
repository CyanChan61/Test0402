using System;
using System.Collections.Generic;
using RogueCard.Cards;

namespace RogueCard.Map
{
    /// <summary>
    /// The binary tree map structure. Tracks the current player position
    /// and exposes navigation and swap operations.
    /// </summary>
    public class MapTree
    {
        public MapNode Root { get; }
        public MapNode CurrentNode { get; private set; }
        public int MaxDepth { get; }

        public event Action<MapNode> OnNodeChanged;

        public MapTree(MapNode root, int maxDepth)
        {
            Root = root;
            MaxDepth = maxDepth;
            CurrentNode = root;
            root.SetState(NodeState.Current);
        }

        /// <summary>
        /// Returns the two children of the current node.
        /// One or both may be null if at a leaf.
        /// </summary>
        public (MapNode childA, MapNode childB) GetCurrentChildren()
        {
            return (CurrentNode.ChildA, CurrentNode.ChildB);
        }

        /// <summary>
        /// Move the player to a target node. Target must be a direct child
        /// of the current node.
        /// </summary>
        public bool MoveToNode(MapNode target)
        {
            if (target == null) return false;
            if (target != CurrentNode.ChildA && target != CurrentNode.ChildB)
                return false;

            // Lock the sibling (path not taken)
            var sibling = target.GetSibling();
            if (sibling != null && sibling.State != NodeState.Visited)
                sibling.SetState(NodeState.Locked);

            CurrentNode.SetState(NodeState.Visited);
            CurrentNode = target;
            CurrentNode.SetState(NodeState.Current);

            OnNodeChanged?.Invoke(CurrentNode);
            return true;
        }

        /// <summary>
        /// Replace the card on a target node with a new card.
        /// Returns the displaced card.
        /// </summary>
        public PlaceCardInstance ReplaceNodeCard(MapNode target, PlaceCardInstance newCard)
        {
            if (target == null || newCard == null) return null;
            target.ReplaceCard(newCard, out var displaced);
            return displaced;
        }

        public bool IsAtLeaf() => CurrentNode.IsLeaf;

        public bool IsAtBoss() =>
            CurrentNode.IsLeaf && CurrentNode.Card?.PlaceType == Cards.PlaceType.Boss;

        public int CurrentDepth => CurrentNode.Depth;

        /// <summary>
        /// Returns all nodes on the path from root to the given node.
        /// </summary>
        public List<MapNode> GetPathToRoot(MapNode node)
        {
            var path = new List<MapNode>();
            var current = node;
            while (current != null)
            {
                path.Add(current);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Collect all nodes in the tree (BFS).
        /// </summary>
        public List<MapNode> GetAllNodes()
        {
            var result = new List<MapNode>();
            var queue = new Queue<MapNode>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                result.Add(node);
                if (node.ChildA != null) queue.Enqueue(node.ChildA);
                if (node.ChildB != null) queue.Enqueue(node.ChildB);
            }
            return result;
        }
    }
}
