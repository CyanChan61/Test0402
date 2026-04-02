using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueCard.Core.States
{
    /// <summary>
    /// First state. Verifies setup and transitions to the main menu.
    /// </summary>
    public class StateBootstrap : GameState
    {
        public override void OnEnter()
        {
            Debug.Log("[Bootstrap] Initializing...");
            // ScriptableObjects are already loaded by Unity.
            // If additional async loading is needed, add it here.
            Game.StateMachine.TransitionTo<StateMainMenu>();
        }

        public override void OnExit() { }
    }
}
