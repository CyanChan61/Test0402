using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Player;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UIGameOverPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI summaryText;
        [SerializeField] private Button newRunButton;
        [SerializeField] private Button menuButton;

        private void Start()
        {
            newRunButton?.onClick.AddListener(OnNewRun);
            menuButton?.onClick.AddListener(OnMenu);
        }

        public void Show(bool victory, PlayerState player)
        {
            if (titleText) titleText.text = victory ? "VICTORY!" : "DEFEAT";
            if (summaryText)
                summaryText.text = $"HP: {player.Stats.CurrentHp}/{player.Stats.MaxHp}\n" +
                                   $"Gold: {player.Gold}\n" +
                                   $"Depth: {Core.GameManager.Instance?.Map?.CurrentDepth}";
        }

        private void OnNewRun()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateGameOver>()?.OnNewRun();
        }

        private void OnMenu()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateGameOver>()?.OnReturnToMenu();
        }
    }
}
