using UnityEngine;
using RogueCard.Cards;
using RogueCard.Map;

namespace RogueCard.Core.States
{
    /// <summary>
    /// Applies the player's decision atomically: swap cards + move player.
    /// Plays the travel animation, then triggers the place effect.
    /// </summary>
    public class StateTransition : GameState
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
            Debug.Log($"[Transition] Executing decision: {_decision}");
            Game.ExecuteDecision(_decision, _nodeA, _handCard, _nodeB);
            // ExecuteDecision handles movement, events, and place effect dispatch
            // The place effect will trigger the next state transition
        }

        public override void OnExit() { }
    }
}
