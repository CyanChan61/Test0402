using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UIShopPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Button leaveButton;

        private void Start()
        {
            leaveButton?.onClick.AddListener(OnLeave);
        }

        public void Refresh(int gold)
        {
            if (goldText) goldText.text = $"Your gold: {gold}";
        }

        private void OnLeave()
        {
            Core.GameManager.Instance.StateMachine.GetState<StateShop>()?.OnLeaveShop();
        }
    }
}
