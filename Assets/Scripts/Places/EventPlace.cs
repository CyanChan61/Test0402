using RogueCard.Cards;

namespace RogueCard.Places
{
    public class EventPlace : PlaceEffect
    {
        public override PlaceType Type => PlaceType.Event;

        public override void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            stateMachine.TransitionTo<Core.States.StateEvent>();
        }

        public override string GetDescription() => "Something unusual is happening here.";
    }
}
