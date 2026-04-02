using UnityEngine;

namespace RogueCard.Core.States
{
    public class StateGameOver : GameState
    {
        public bool IsVictory { get; private set; }

        public void SetVictory(bool victory) => IsVictory = victory;

        public override void OnEnter()
        {
            // Determine outcome from run end event
            Debug.Log($"[GameOver] {(IsVictory ? "VICTORY!" : "DEFEAT.")}");
            Game.UIManager?.HideAll();
            Game.UIManager?.ShowPanel<UI.Panels.UIGameOverPanel>();
            Game.UIManager?.GetPanel<UI.Panels.UIGameOverPanel>()?.Show(IsVictory, Game.Player);
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UIGameOverPanel>();
        }

        public void OnReturnToMenu()
        {
            Game.StateMachine.TransitionTo<StateMainMenu>();
        }

        public void OnNewRun()
        {
            Game.StartNewRun();
        }
    }
}
