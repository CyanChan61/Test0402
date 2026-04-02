using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Cards;
using RogueCard.Map;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UIDecisionPanel : UIPanel
    {
        [Header("Buttons")]
        [SerializeField] private Button goToAButton;
        [SerializeField] private Button goToBButton;
        [SerializeField] private TextMeshProUGUI nodeAInfo;
        [SerializeField] private TextMeshProUGUI nodeBInfo;

        [Header("Hand Card Selection")]
        [SerializeField] private Transform handCardContainer;
        [SerializeField] private Components.UICardView cardViewPrefab;

        private MapNode _nodeA;
        private MapNode _nodeB;
        private PlaceCardInstance _selectedHandCard;

        private void Start()
        {
            goToAButton?.onClick.AddListener(OnGoToA);
            goToBButton?.onClick.AddListener(OnGoToB);
        }

        public void Refresh(MapNode nodeA, MapNode nodeB, Hand hand)
        {
            _nodeA = nodeA;
            _nodeB = nodeB;
            _selectedHandCard = null;

            if (nodeAInfo) nodeAInfo.text = $"A: {nodeA?.Card?.DisplayName ?? "???"}";
            if (nodeBInfo) nodeBInfo.text = "B: ???";

            RefreshHandCards(hand);
        }

        private void RefreshHandCards(Hand hand)
        {
            if (handCardContainer == null) return;

            foreach (Transform child in handCardContainer)
                Destroy(child.gameObject);

            foreach (var card in hand.Cards)
            {
                if (cardViewPrefab == null) continue;
                var view = Instantiate(cardViewPrefab, handCardContainer);
                view.Setup(card);
                view.OnSelected += OnHandCardSelected;
            }
        }

        private void OnHandCardSelected(PlaceCardInstance card)
        {
            _selectedHandCard = card;
            Debug.Log($"[DecisionPanel] Selected hand card: {card}");
        }

        private void OnGoToA()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateDecisionPhase>()?.OnGoToA();
        }

        private void OnGoToB()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateDecisionPhase>()?.OnGoToB();
        }

        public void OnReplaceGoToC()
        {
            if (_selectedHandCard == null) { Debug.Log("[DecisionPanel] No hand card selected."); return; }
            Core.GameManager.Instance.StateMachine.GetState<StateDecisionPhase>()?.OnReplaceAGoToC(_selectedHandCard);
        }

        public void OnReplaceGoToB()
        {
            if (_selectedHandCard == null) { Debug.Log("[DecisionPanel] No hand card selected."); return; }
            Core.GameManager.Instance.StateMachine.GetState<StateDecisionPhase>()?.OnReplaceAGoToB(_selectedHandCard);
        }
    }
}
