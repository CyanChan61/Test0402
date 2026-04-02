using RogueCard.Places;

namespace RogueCard.Cards
{
    /// <summary>
    /// Runtime instance of a PlaceCard. Wraps the ScriptableObject definition
    /// and tracks per-instance state (revealed, effect resolved).
    /// </summary>
    public class PlaceCardInstance
    {
        public PlaceCard Definition { get; }
        public bool IsRevealed { get; private set; }
        public PlaceEffect Effect { get; }

        public string DisplayName => Definition.displayName;
        public PlaceType PlaceType => Definition.placeType;

        public PlaceCardInstance(PlaceCard definition)
        {
            Definition = definition;
            IsRevealed = false;
            Effect = PlaceEffectFactory.Create(definition.placeType);
        }

        public void Reveal()
        {
            IsRevealed = true;
        }

        public void ExecuteEffect(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            Effect?.Execute(player, stateMachine);
        }

        public override string ToString() =>
            $"[{(IsRevealed ? Definition.displayName : "???")} ({Definition.placeType})]";
    }
}
