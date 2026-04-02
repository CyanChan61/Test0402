using UnityEngine;
using UnityEngine.UI;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UIMainMenuPanel : UIPanel
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            newGameButton?.onClick.AddListener(OnNewGame);
            continueButton?.onClick.AddListener(OnContinue);
            quitButton?.onClick.AddListener(OnQuit);
        }

        private void OnNewGame()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateMainMenu>()?.OnNewGame();
        }

        private void OnContinue()
        {
            // TODO: Load saved run
            Debug.Log("[MainMenu] Continue not yet implemented.");
        }

        private void OnQuit()
        {
            Application.Quit();
        }
    }
}
