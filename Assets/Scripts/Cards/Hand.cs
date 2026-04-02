using System;
using System.Collections.Generic;

namespace RogueCard.Cards
{
    /// <summary>
    /// Manages the player's hand of place cards.
    /// </summary>
    public class Hand
    {
        private readonly List<PlaceCardInstance> _cards = new();

        public IReadOnlyList<PlaceCardInstance> Cards => _cards;
        public int MaxSize { get; }
        public int Count => _cards.Count;
        public bool IsFull => _cards.Count >= MaxSize;
        public bool IsEmpty => _cards.Count == 0;

        public event Action<PlaceCardInstance> OnCardAdded;
        public event Action<PlaceCardInstance> OnCardRemoved;

        public Hand(int maxSize = 6)
        {
            MaxSize = maxSize;
        }

        public bool TryAddCard(PlaceCardInstance card)
        {
            if (card == null || IsFull) return false;
            _cards.Add(card);
            OnCardAdded?.Invoke(card);
            return true;
        }

        public bool TryRemoveCard(PlaceCardInstance card)
        {
            if (card == null || !_cards.Contains(card)) return false;
            _cards.Remove(card);
            OnCardRemoved?.Invoke(card);
            return true;
        }

        public PlaceCardInstance GetCard(int index)
        {
            if (index < 0 || index >= _cards.Count) return null;
            return _cards[index];
        }

        public bool Contains(PlaceCardInstance card) => _cards.Contains(card);

        public void Clear()
        {
            var copy = new List<PlaceCardInstance>(_cards);
            foreach (var card in copy)
                TryRemoveCard(card);
        }
    }
}
