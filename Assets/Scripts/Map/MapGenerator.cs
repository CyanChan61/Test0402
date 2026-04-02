using System;
using RogueCard.Cards;
using RogueCard.Data;

namespace RogueCard.Map
{
    /// <summary>
    /// Generates a binary tree map with procedurally assigned place cards.
    /// The tree is deterministic given the same seed + RunConfig.
    /// </summary>
    public class MapGenerator
    {
        private Deck _deck;
        private RunConfig _config;
        private Random _rng;

        public MapTree Generate(Deck deck, RunConfig config, int seed)
        {
            _deck = deck;
            _config = config;
            _rng = new Random(seed);

            MapNode.ResetIdCounter();

            var root = BuildSubtree(depth: 0, parent: null);
            AssignStartCard(root);

            var tree = new MapTree(root, config.treeMaxDepth);
            return tree;
        }

        private MapNode BuildSubtree(int depth, MapNode parent)
        {
            var node = new MapNode(depth, parent);

            if (depth < _config.treeMaxDepth)
            {
                var childA = BuildSubtree(depth + 1, node);
                var childB = BuildSubtree(depth + 1, node);
                node.SetChildren(childA, childB);
            }

            // Assign card based on depth
            var card = SelectCardForDepth(depth, node.IsLeaf);
            if (card != null)
                node.AssignCard(card.CreateInstance());

            return node;
        }

        private PlaceCard SelectCardForDepth(int depth, bool isLeaf)
        {
            float normalizedDepth = _config.treeMaxDepth > 0
                ? (float)depth / _config.treeMaxDepth
                : 0f;

            if (isLeaf)
                return _deck.DrawByType(PlaceType.Boss) ?? _deck.DrawWeighted(normalizedDepth);

            // Exclude Boss and Start from non-leaf, non-root nodes
            var allowed = new[]
            {
                PlaceType.Combat, PlaceType.Elite, PlaceType.Shop,
                PlaceType.Rest, PlaceType.Event, PlaceType.Treasure
            };

            return _deck.DrawWeighted(normalizedDepth, allowed);
        }

        private void AssignStartCard(MapNode root)
        {
            var startCard = _deck.DrawByType(PlaceType.Start);
            if (startCard != null)
                root.AssignCard(startCard.CreateInstance());

            // Reveal the root so the player can see where they start
            root.Card?.Reveal();
        }
    }
}
