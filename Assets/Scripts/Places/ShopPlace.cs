using RogueCard.Cards;

namespace RogueCard.Places
{
    public class ShopPlace : PlaceEffect
    {
        public override PlaceType Type => PlaceType.Shop;

        public override void Execute(Player.PlayerState player, Core.GameStateMachine stateMachine)
        {
            stateMachine.TransitionTo<Core.States.StateShop>();
        }

        public override string GetDescription() => "A wandering merchant offers wares.";
    }
}
