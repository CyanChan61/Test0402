using UnityEngine;
using RogueCard.Cards;

namespace RogueCard.Core.States
{
    public class StateShop : GameState
    {
        private const int CardCost = 20;

        public override void OnEnter()
        {
            Debug.Log("[Shop] Open for business.");
            Game.UIManager?.ShowPanel<UI.Panels.UIShopPanel>();
            Game.UIManager?.GetPanel<UI.Panels.UIShopPanel>()?.Refresh(Game.Player.Gold);
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UIShopPanel>();
        }

        /// <summary>Called by UI: Buy a place card for hand.</summary>
        public void OnBuyCard(PlaceCard card)
        {
            if (Game.Player.Gold < CardCost)
            {
                Debug.Log("[Shop] Not enough gold.");
                return;
            }
            if (Game.Player.Hand.IsFull)
            {
                Debug.Log("[Shop] Hand is full.");
                return;
            }

            Game.Player.ModifyGold(-CardCost);
            Game.Player.TryAddCardToHand(card.CreateInstance());
            Debug.Log($"[Shop] Bought {card.displayName} for {CardCost}g.");
        }

        public void OnLeaveShop()
        {
            Game.StateMachine.TransitionTo<StateMapNavigation>();
        }
    }
}
