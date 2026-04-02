using UnityEngine;
using RogueCard.Cards;
using RogueCard.Map;

namespace RogueCard.Core.States
{
    /// <summary>
    /// Shows a confirmation UI before committing a swap.
    /// Player can confirm or cancel back to DecisionPhase.
    /// </summary>
    public class StateSwapConfirm : GameState
    {
        private PlayerDecision _decision;
        private MapNode _nodeA;
        private MapNode _nodeB;
        private PlaceCardInstance _handCard;

        public void Setup(PlayerDecision decision, MapNode nodeA, MapNode nodeB, PlaceCardInstance handCard)
        {
            _decision = decision;
            _nodeA = nodeA;
            _nodeB = nodeB;
            _handCard = handCard;
        }

        public override void OnEnter()
        {
            Debug.Log($"[SwapConfirm] Confirming {_decision} with hand card: {_handCard}");
            var panel = Game.UIManager?.GetPanel<UI.Panels.UISwapPreviewPanel>();
            if (panel != null)
            {
                panel.Show(_decision, _nodeA, _handCard);
                Game.UIManager?.ShowPanel<UI.Panels.UISwapPreviewPanel>();
            }
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UISwapPreviewPanel>();
        }

        public void OnConfirm()
        {
            var transitionState = Game.StateMachine.GetState<StateTransition>();
            transitionState.Setup(_decision, _nodeA, _nodeB, _handCard);
            Game.StateMachine.TransitionTo<StateTransition>();
        }

        public void OnCancel()
        {
            Game.StateMachine.TransitionTo<StateDecisionPhase>();
        }
    }
}
