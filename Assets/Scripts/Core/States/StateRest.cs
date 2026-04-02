using UnityEngine;

namespace RogueCard.Core.States
{
    public class StateRest : GameState
    {
        public override void OnEnter()
        {
            int healAmount = Game.ActiveConfig.restHealAmount;
            Game.Player.ApplyHeal(healAmount);
            Debug.Log($"[Rest] Player healed {healAmount} HP. Current HP: {Game.Player.Stats.CurrentHp}");

            Game.UIManager?.ShowPanel<UI.Panels.UIRestPanel>();
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UIRestPanel>();
        }

        public void OnContinue()
        {
            Game.StateMachine.TransitionTo<StateMapNavigation>();
        }
    }
}
