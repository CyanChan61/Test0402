using RogueCard.Cards;

namespace RogueCard.Places
{
    public class RestPlace : PlaceEffect
    {
        public override PlaceType Type => PlaceType.Rest;

        public override void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            stateMachine.TransitionTo<Core.States.StateRest>();
        }

        public override string GetDescription() => "A campfire. Rest and recover.";
    }
}
