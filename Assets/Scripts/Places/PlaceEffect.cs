using RogueCard.Cards;

namespace RogueCard.Places
{
    /// <summary>
    /// Abstract base for what happens when a player lands on a place.
    /// Each concrete subclass triggers the appropriate game state.
    /// </summary>
    public abstract class PlaceEffect
    {
        public abstract PlaceType Type { get; }

        public abstract void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine);

        public virtual string GetDescription() => string.Empty;
    }

    /// <summary>
    /// Factory that creates the correct PlaceEffect for a given PlaceType.
    /// </summary>
    public static class PlaceEffectFactory
    {
        public static PlaceEffect Create(PlaceType type)
        {
            return type switch
            {
                PlaceType.Start    => new StartPlace(),
                PlaceType.Combat   => new CombatPlace(),
                PlaceType.Elite    => new ElitePlace(),
                PlaceType.Boss     => new BossPlace(),
                PlaceType.Shop     => new ShopPlace(),
                PlaceType.Rest     => new RestPlace(),
                PlaceType.Event    => new EventPlace(),
                PlaceType.Treasure => new TreasurePlace(),
                _                  => new CombatPlace()
            };
        }
    }
}
