using UnityEngine;

namespace RogueCard.Core.States
{
    public class StateTreasure : GameState
    {
        public override void OnEnter()
        {
            var config = Game.ActiveConfig;
            var rng = new System.Random();
            int gold = rng.Next(config.treasureGoldMin, config.treasureGoldMax + 1);

            Game.Player.ModifyGold(gold);
            Debug.Log($"[Treasure] Found {gold} gold!");

            Game.UIManager?.ShowPanel<UI.Panels.UITreasurePanel>();
            Game.UIManager?.GetPanel<UI.Panels.UITreasurePanel>()?.ShowReward(gold);
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UITreasurePanel>();
        }

        public void OnContinue()
        {
            Game.StateMachine.TransitionTo<StateMapNavigation>();
        }
    }
}
