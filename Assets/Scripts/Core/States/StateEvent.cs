using UnityEngine;

namespace RogueCard.Core.States
{
    public class StateEvent : GameState
    {
        public override void OnEnter()
        {
            Debug.Log("[Event] Random event triggered.");
            // TODO: Load a random EventData ScriptableObject and display its text/choices
            Game.UIManager?.ShowPanel<UI.Panels.UIEventPanel>();
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UIEventPanel>();
        }

        public void OnChoiceSelected(int choiceIndex)
        {
            // TODO: Apply event outcome based on choice
            Debug.Log($"[Event] Player chose option {choiceIndex}.");
            Game.StateMachine.TransitionTo<StateMapNavigation>();
        }
    }
}
