using UnityEngine;
using RogueCard.Map;

namespace RogueCard.Core.States
{
    /// <summary>
    /// Player is at current node. Presents two hidden child nodes to choose from.
    /// Waits for the player to select one to reveal.
    /// </summary>
    public class StateMapNavigation : GameState
    {
        public MapNode NodeA { get; private set; }
        public MapNode NodeB { get; private set; }

        public override void OnEnter()
        {
            Debug.Log("[MapNavigation] Presenting choices.");

            // Check if at a leaf with no children
            if (Game.Navigator.IsAtLeaf)
            {
                Debug.Log("[MapNavigation] At leaf — triggering place effect directly.");
                Game.TriggerPlaceEffect(Game.Map.CurrentNode);
                return;
            }

            (NodeA, NodeB) = Game.Navigator.GetChoices();

            Game.UIManager?.ShowPanel<UI.Panels.UIMapPanel>();
            Game.UIManager?.ShowPanel<UI.Panels.UIHUDPanel>();
            Game.UIManager?.GetPanel<UI.Panels.UIMapPanel>()?.RefreshChoices(NodeA, NodeB);
        }

        public override void OnExit()
        {
            // Keep map and HUD visible across states
        }

        /// <summary>Called by UIMapPanel when player taps a node to reveal.</summary>
        public void OnPlayerSelectReveal(MapNode node)
        {
            var revealState = Game.StateMachine.GetState<StateRevealPhase>();
            revealState.SetNodeToReveal(node, NodeA, NodeB);
            Game.StateMachine.TransitionTo<StateRevealPhase>();
        }
    }
}
