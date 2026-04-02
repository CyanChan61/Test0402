using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Map;

namespace RogueCard.UI.Components
{
    /// <summary>
    /// Visual representation of a map node (black card or revealed place).
    /// </summary>
    public class UINodeView : MonoBehaviour
    {
        [SerializeField] private GameObject hiddenFace;
        [SerializeField] private GameObject revealedFace;
        [SerializeField] private TextMeshProUGUI placeNameText;
        [SerializeField] private Image placeIcon;
        [SerializeField] private Image nodeStateIndicator;

        public MapNode Node { get; private set; }

        public void Setup(MapNode node)
        {
            Node = node;
            Refresh();
        }

        public void Refresh()
        {
            if (Node == null) return;

            bool revealed = Node.State == NodeState.Revealed
                         || Node.State == NodeState.Current
                         || Node.State == NodeState.Visited;

            hiddenFace?.SetActive(!revealed);
            revealedFace?.SetActive(revealed);

            if (revealed && Node.Card != null)
            {
                if (placeNameText) placeNameText.text = Node.Card.DisplayName;
                if (placeIcon && Node.Card.Definition.artwork)
                    placeIcon.sprite = Node.Card.Definition.artwork;
            }

            // State indicator color
            if (nodeStateIndicator)
            {
                nodeStateIndicator.color = Node.State switch
                {
                    NodeState.Current  => Color.yellow,
                    NodeState.Visited  => Color.gray,
                    NodeState.Locked   => new Color(0.3f, 0.3f, 0.3f),
                    NodeState.Revealed => Color.white,
                    _                  => Color.black
                };
            }
        }
    }
}
