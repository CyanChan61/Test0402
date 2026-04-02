using RogueCard.Cards;

namespace RogueCard.Places
{
    public class StartPlace : PlaceEffect
    {
        public override PlaceType Type => PlaceType.Start;

        public override void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            // Starting node — just proceed to map navigation
            stateMachine.TransitionTo<Core.States.StateMapNavigation>();
        }

        public override string GetDescription() => "The journey begins here.";
    }
}
