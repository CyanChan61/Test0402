using RogueCard.Cards;

namespace RogueCard.Places
{
    public class ElitePlace : PlaceEffect
    {
        public override PlaceType Type => PlaceType.Elite;

        public override void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            stateMachine.TransitionTo<Core.States.StateCombat>();
            // StateCombat will check current node type to determine enemy difficulty
        }

        public override string GetDescription() => "A powerful foe guards this place.";
    }
}
