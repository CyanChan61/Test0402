using UnityEngine;
using RogueCard.Map;

namespace RogueCard.Core.States
{
    /// <summary>
    /// Reveals the selected node, plays the flip animation, then goes to Decision.
    /// </summary>
    public class StateRevealPhase : GameState
    {
        public MapNode RevealedNode { get; private set; }
        public MapNode OtherNode { get; private set; }

        public void SetNodeToReveal(MapNode toReveal, MapNode nodeA, MapNode nodeB)
        {
            RevealedNode = toReveal;
            OtherNode = (toReveal == nodeA) ? nodeB : nodeA;
        }

        public override void OnEnter()
        {
            if (RevealedNode == null)
            {
                Debug.LogWarning("[RevealPhase] No node set to reveal. Returning to navigation.");
                Game.StateMachine.TransitionTo<StateMapNavigation>();
                return;
            }

            Debug.Log($"[RevealPhase] Revealing {RevealedNode}");
            bool revealed = Game.RevealController.TryReveal(RevealedNode);

            if (!revealed)
            {
                Debug.LogWarning("[RevealPhase] Reveal failed. Returning to navigation.");
                Game.StateMachine.TransitionTo<StateMapNavigation>();
                return;
            }

            // Update UI — node flip animation is driven by the event bus (OnNodeRevealed)
            // Transition to decision after a short delay (animation time)
            // For now, transition immediately; hook in animation callback later
            Game.StateMachine.TransitionTo<StateDecisionPhase>();
        }

        public override void OnExit() { }
    }
}
