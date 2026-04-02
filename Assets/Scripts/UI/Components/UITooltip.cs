using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace RogueCard.UI.Components
{
    /// <summary>
    /// Shows a tooltip when hovering over a UI element.
    /// </summary>
    public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string tooltipText;
        [SerializeField] private GameObject tooltipPanel;
        [SerializeField] private TextMeshProUGUI tooltipLabel;

        public void SetText(string text)
        {
            tooltipText = text;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipPanel) tooltipPanel.SetActive(true);
            if (tooltipLabel) tooltipLabel.text = tooltipText;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltipPanel) tooltipPanel.SetActive(false);
        }
    }
}
