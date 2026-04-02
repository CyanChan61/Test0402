using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RogueCard.Cards;
using RogueCard.Core.States;

namespace RogueCard.UI.Panels
{
    public class UICombatPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI enemyNameText;
        [SerializeField] private TextMeshProUGUI enemyHpText;
        [SerializeField] private Slider enemyHpBar;
        [SerializeField] private Button attackButton;

        private void Start()
        {
            attackButton?.onClick.AddListener(OnAttack);
        }

        public void Refresh(PlaceType enemyType, int hp, int maxHp)
        {
            if (enemyNameText) enemyNameText.text = enemyType.ToString();
            if (enemyHpText) enemyHpText.text = $"{hp}/{maxHp}";
            if (enemyHpBar && maxHp > 0) enemyHpBar.value = (float)hp / maxHp;
        }

        private void OnAttack()
        {
            // Player attack power: base 10 for now (can be expanded with equipment/cards)
            Core.GameManager.Instance.StateMachine.GetState<StateCombat>()?.OnPlayerAttack(10);
        }
    }
}
