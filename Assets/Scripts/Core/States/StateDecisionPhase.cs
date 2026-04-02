using UnityEngine;
using RogueCard.Cards;
using RogueCard.Map;

namespace RogueCard.Core.States
{
    /// <summary>
    /// Shows the player their hand + the two node choices, and the four action buttons.
    /// Waits for player to commit a decision.
    /// </summary>
    public class StateDecisionPhase : GameState
    {
        private MapNode _nodeA;
        private MapNode _nodeB;
        private PlaceCardInstance _selectedHandCard;

        public override void OnEnter()
        {
            var revealState = Game.StateMachine.GetState<StateRevealPhase>();
            _nodeA = revealState.RevealedNode;
            _nodeB = revealState.OtherNode;
            _selectedHandCard = null;

            Debug.Log($"[Decision] NodeA={_nodeA?.Card?.DisplayName}, NodeB=???");

            Game.UIManager?.ShowPanel<UI.Panels.UIDecisionPanel>();
            Game.UIManager?.ShowPanel<UI.Panels.UIHandPanel>();

            var decisionPanel = Game.UIManager?.GetPanel<UI.Panels.UIDecisionPanel>();
            decisionPanel?.Refresh(_nodeA, _nodeB, Game.Player.Hand);
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UIDecisionPanel>();
            Game.UIManager?.HidePanel<UI.Panels.UIHandPanel>();
        }

        // ── Called by UI buttons ─────────────────────────────────────────────

        public void OnGoToA()
        {
            CommitDecision(PlayerDecision.GoToA, null);
        }

        public void OnGoToB()
        {
            CommitDecision(PlayerDecision.GoToB, null);
        }

        public void OnReplaceAGoToC(PlaceCardInstance handCard)
        {
            if (handCard == null || !Game.SwapResolver.CanSwap(handCard, _nodeA))
            {
                Debug.LogWarning("[Decision] Invalid swap for ReplaceAGoToC.");
                return;
            }
            _selectedHandCard = handCard;
            var confirmState = Game.StateMachine.GetState<StateSwapConfirm>();
            confirmState.Setup(PlayerDecision.ReplaceAGoToC, _nodeA, _nodeB, handCard);
            Game.StateMachine.TransitionTo<StateSwapConfirm>();
        }

        public void OnReplaceAGoToB(PlaceCardInstance handCard)
        {
            if (handCard == null || !Game.SwapResolver.CanSwap(handCard, _nodeA))
            {
                Debug.LogWarning("[Decision] Invalid swap for ReplaceAGoToB.");
                return;
            }
            _selectedHandCard = handCard;
            var confirmState = Game.StateMachine.GetState<StateSwapConfirm>();
            confirmState.Setup(PlayerDecision.ReplaceAGoToB, _nodeA, _nodeB, handCard);
            Game.StateMachine.TransitionTo<StateSwapConfirm>();
        }

        private void CommitDecision(PlayerDecision decision, PlaceCardInstance handCard)
        {
            var transitionState = Game.StateMachine.GetState<StateTransition>();
            transitionState.Setup(decision, _nodeA, _nodeB, handCard);
            Game.StateMachine.TransitionTo<StateTransition>();
        }
    }
}
