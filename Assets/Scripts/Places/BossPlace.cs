using RogueCard.Cards;

namespace RogueCard.Places
{
    public class BossPlace : PlaceEffect
    {
        public override PlaceType Type => PlaceType.Boss;

        public override void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            stateMachine.TransitionTo<Core.States.StateCombat>();
            // StateCombat will detect Boss type and trigger victory on win
        }

        public override string GetDescription() => "The final guardian of this path awaits.";
    }
}
