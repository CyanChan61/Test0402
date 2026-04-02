using System;
using System.Collections.Generic;

namespace RogueCard.Utility
{
    /// <summary>
    /// Utility for weighted random selection from a list of items.
    /// </summary>
    public class WeightedRandom<T>
    {
        private readonly List<(T item, float weight)> _entries = new();
        private readonly Random _rng;
        private float _totalWeight;

        public WeightedRandom(Random rng)
        {
            _rng = rng;
        }

        public void Add(T item, float weight)
        {
            if (weight <= 0f) return;
            _entries.Add((item, weight));
            _totalWeight += weight;
        }

        public void Clear()
        {
            _entries.Clear();
            _totalWeight = 0f;
        }

        public bool HasEntries => _entries.Count > 0;

        public T Pick()
        {
            if (_entries.Count == 0)
                throw new InvalidOperationException("WeightedRandom has no entries.");

            float roll = (float)_rng.NextDouble() * _totalWeight;
            float cumulative = 0f;
            foreach (var (item, weight) in _entries)
            {
                cumulative += weight;
                if (roll <= cumulative) return item;
            }
            return _entries[_entries.Count - 1].item;
        }
    }
}
