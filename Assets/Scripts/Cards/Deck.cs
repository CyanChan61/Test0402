using System.Collections.Generic;
using UnityEngine;

namespace RogueCard.Cards
{
    /// <summary>
    /// A pool of PlaceCard definitions used by MapGenerator to populate tree nodes.
    /// </summary>
    public class Deck
    {
        private readonly List<PlaceCard> _allCards;
        private readonly System.Random _rng;

        public Deck(IEnumerable<PlaceCard> cards, int seed)
        {
            _allCards = new List<PlaceCard>(cards);
            _rng = new System.Random(seed);
        }

        /// <summary>
        /// Draw a random card matching the given place type.
        /// Returns null if no matching card found.
        /// </summary>
        public PlaceCard DrawByType(PlaceType type)
        {
            var matching = _allCards.FindAll(c => c.placeType == type);
            if (matching.Count == 0) return null;
            return matching[_rng.Next(matching.Count)];
        }

        /// <summary>
        /// Draw a weighted random card based on depth (0-1 normalized).
        /// </summary>
        public PlaceCard DrawWeighted(float normalizedDepth, PlaceType[] allowedTypes = null)
        {
            var candidates = new List<(PlaceCard card, float weight)>();
            foreach (var card in _allCards)
            {
                if (allowedTypes != null && System.Array.IndexOf(allowedTypes, card.placeType) < 0)
                    continue;
                float w = card.spawnWeightCurve.Evaluate(normalizedDepth);
                if (w > 0f) candidates.Add((card, w));
            }

            if (candidates.Count == 0) return null;

            float total = 0f;
            foreach (var (_, w) in candidates) total += w;

            float roll = (float)_rng.NextDouble() * total;
            float cumulative = 0f;
            foreach (var (card, w) in candidates)
            {
                cumulative += w;
                if (roll <= cumulative) return card;
            }

            return candidates[candidates.Count - 1].card;
        }

        public int Count => _allCards.Count;
    }
}
