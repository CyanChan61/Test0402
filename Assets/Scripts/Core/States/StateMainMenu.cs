using UnityEngine;

namespace RogueCard.Core.States
{
    public class StateMainMenu : GameState
    {
        public override void OnEnter()
        {
            Debug.Log("[MainMenu] Showing main menu.");
            Game.UIManager?.ShowPanel<UI.Panels.UIMainMenuPanel>();
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UIMainMenuPanel>();
        }

        /// <summary>Called by UI button: New Game.</summary>
        public void OnNewGame()
        {
            Game.StartNewRun();
        }
    }
}
