using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Cards;
using RogueCard.Map;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UISwapPreviewPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private void Start()
        {
            confirmButton?.onClick.AddListener(OnConfirm);
            cancelButton?.onClick.AddListener(OnCancel);
        }

        public void Show(PlayerDecision decision, MapNode nodeA, PlaceCardInstance handCard)
        {
            string desc = decision switch
            {
                PlayerDecision.ReplaceAGoToC =>
                    $"Place [{handCard?.DisplayName}] where [{nodeA?.Card?.DisplayName}] was.\n" +
                    $"[{nodeA?.Card?.DisplayName}] returns to your hand.\n" +
                    $"You travel to [{handCard?.DisplayName}].",
                PlayerDecision.ReplaceAGoToB =>
                    $"Place [{handCard?.DisplayName}] where [{nodeA?.Card?.DisplayName}] was.\n" +
                    $"[{handCard?.DisplayName}] is LOST.\n" +
                    $"You travel to the unknown node.",
                _ => string.Empty
            };

            if (descriptionText) descriptionText.text = desc;
        }

        private void OnConfirm()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateSwapConfirm>()?.OnConfirm();
        }

        private void OnCancel()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateSwapConfirm>()?.OnCancel();
        }
    }
}
