using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Map;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UIMapPanel : UIPanel
    {
        [Header("Node Buttons")]
        [SerializeField] private Button nodeAButton;
        [SerializeField] private Button nodeBButton;
        [SerializeField] private TextMeshProUGUI nodeALabel;
        [SerializeField] private TextMeshProUGUI nodeBLabel;

        private MapNode _nodeA;
        private MapNode _nodeB;

        private void Start()
        {
            nodeAButton?.onClick.AddListener(() => OnSelectNode(_nodeA));
            nodeBButton?.onClick.AddListener(() => OnSelectNode(_nodeB));
        }

        public void RefreshChoices(MapNode nodeA, MapNode nodeB)
        {
            _nodeA = nodeA;
            _nodeB = nodeB;

            if (nodeALabel) nodeALabel.text = "???";
            if (nodeBLabel) nodeBLabel.text = "???";

            nodeAButton?.gameObject.SetActive(nodeA != null);
            nodeBButton?.gameObject.SetActive(nodeB != null);
        }

        /// <summary>Update a node's label after it's revealed.</summary>
        public void OnNodeRevealed(MapNode node)
        {
            if (node == _nodeA && nodeALabel)
                nodeALabel.text = node.Card?.DisplayName ?? "???";
            else if (node == _nodeB && nodeBLabel)
                nodeBLabel.text = node.Card?.DisplayName ?? "???";
        }

        private void OnSelectNode(MapNode node)
        {
            Core.GameManager.Instance.StateMachine.GetState<StateMapNavigation>()?
                .OnPlayerSelectReveal(node);
        }
    }
}
