using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RogueCard.UI.Panels
{
    public class UIHUDPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Slider hpBar;

        private void Update()
        {
            var player = Core.GameManager.Instance?.Player;
            if (player == null) return;

            if (hpText) hpText.text = $"HP: {player.Stats.CurrentHp}/{player.Stats.MaxHp}";
            if (goldText) goldText.text = $"Gold: {player.Gold}";
            if (hpBar) hpBar.value = player.Stats.HpPercent;
        }
    }
}
