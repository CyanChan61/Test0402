using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UITreasurePanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI rewardText;
        [SerializeField] private Button continueButton;

        private void Start()
        {
            continueButton?.onClick.AddListener(OnContinue);
        }

        public void ShowReward(int gold)
        {
            if (rewardText) rewardText.text = $"You found {gold} gold!";
        }

        private void OnContinue()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateTreasure>()?.OnContinue();
        }
    }
}
