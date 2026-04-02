using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UIEventPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI eventText;
        [SerializeField] private Button choice1Button;
        [SerializeField] private Button choice2Button;

        private void Start()
        {
            choice1Button?.onClick.AddListener(() => OnChoice(0));
            choice2Button?.onClick.AddListener(() => OnChoice(1));
        }

        public override void Show()
        {
            base.Show();
            // Placeholder text; wire up EventData ScriptableObjects later
            if (eventText) eventText.text = "A strange event unfolds before you...";
        }

        private void OnChoice(int index)
        {
            Core.GameManager.Instance.StateMachine.GetState<StateEvent>()?.OnChoiceSelected(index);
        }
    }
}
