using RogueCard.Cards;

namespace RogueCard.Places
{
    public class TreasurePlace : PlaceEffect
    {
        public override PlaceType Type => PlaceType.Treasure;

        public override void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            stateMachine.TransitionTo<Core.States.StateTreasure>();
        }

        public override string GetDescription() => "Riches and rewards await the bold.";
    }
}
