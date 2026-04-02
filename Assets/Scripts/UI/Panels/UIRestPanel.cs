using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UIRestPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button continueButton;

        private void Start()
        {
            continueButton?.onClick.AddListener(OnContinue);
        }

        public override void Show()
        {
            base.Show();
            int heal = Core.GameManager.Instance?.ActiveConfig?.restHealAmount ?? 0;
            if (messageText) messageText.text = $"You rest and recover {heal} HP.";
        }

        private void OnContinue()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateRest>()?.OnContinue();
        }
    }
}
