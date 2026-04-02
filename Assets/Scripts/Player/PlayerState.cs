using System;
using RogueCard.Cards;
using RogueCard.Map;

namespace RogueCard.Player
{
    /// <summary>
    /// Holds all runtime state for the player during a run.
    /// </summary>
    public class PlayerState
    {
        public PlayerStats Stats { get; }
        public Hand Hand { get; }

        public int Gold { get; private set; }
        public MapNode Position { get; private set; }

        public event Action<int> OnGoldChanged;

        public bool IsAlive => Stats.IsAlive;

        public PlayerState(int startingHp, int startingGold, int maxHandSize)
        {
            Stats = new PlayerStats(startingHp);
            Hand = new Hand(maxHandSize);
            Gold = startingGold;
        }

        public void MoveTo(MapNode node)
        {
            Position = node;
        }

        public void ModifyGold(int delta)
        {
            Gold = Math.Max(0, Gold + delta);
            OnGoldChanged?.Invoke(Gold);
        }

        public void ApplyDamage(int amount) => Stats.ApplyDamage(amount);
        public void ApplyHeal(int amount) => Stats.ApplyHeal(amount);

        /// <summary>
        /// Add a card to hand. Returns false if hand is full.
        /// </summary>
        public bool TryAddCardToHand(PlaceCardInstance card) => Hand.TryAddCard(card);

        /// <summary>
        /// Remove a card from hand. Returns false if not found.
        /// </summary>
        public bool TryRemoveCardFromHand(PlaceCardInstance card) => Hand.TryRemoveCard(card);
    }
}
