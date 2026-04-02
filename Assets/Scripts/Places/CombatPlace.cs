using RogueCard.Cards;

namespace RogueCard.Places
{
    public class CombatPlace : PlaceEffect
    {
        public override PlaceType Type => PlaceType.Combat;

        public override void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            stateMachine.TransitionTo<Core.States.StateCombat>();
        }

        public override string GetDescription() => "Enemies lurk here. Prepare for battle.";
    }
}
