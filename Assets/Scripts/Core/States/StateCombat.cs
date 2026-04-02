using UnityEngine;
using RogueCard.Cards;

namespace RogueCard.Core.States
{
    /// <summary>
    /// Handles combat encounters (Combat, Elite, Boss).
    /// Simple turn-based combat: player attacks, then enemy attacks.
    /// </summary>
    public class StateCombat : GameState
    {
        private int _enemyHp;
        private int _enemyMaxHp;
        private int _enemyAttack;
        private PlaceType _enemyType;
        private bool _inCombat;

        public override void OnEnter()
        {
            var config = Game.ActiveConfig;
            var currentCard = Game.Map.CurrentNode.Card;
            _enemyType = currentCard?.PlaceType ?? PlaceType.Combat;

            // Scale enemy stats by type
            _enemyMaxHp = _enemyType switch
            {
                PlaceType.Elite => Mathf.RoundToInt(config.baseEnemyHp * config.eliteHpMultiplier),
                PlaceType.Boss  => Mathf.RoundToInt(config.baseEnemyHp * config.bossHpMultiplier),
                _               => config.baseEnemyHp
            };
            _enemyAttack = _enemyType switch
            {
                PlaceType.Elite => Mathf.RoundToInt(config.baseEnemyAttack * config.eliteAttackMultiplier),
                PlaceType.Boss  => Mathf.RoundToInt(config.baseEnemyAttack * config.bossAttackMultiplier),
                _               => config.baseEnemyAttack
            };
            _enemyHp = _enemyMaxHp;
            _inCombat = true;

            Debug.Log($"[Combat] {_enemyType} encounter! HP={_enemyHp} ATK={_enemyAttack}");
            Game.UIManager?.ShowPanel<UI.Panels.UICombatPanel>();
            Game.UIManager?.GetPanel<UI.Panels.UICombatPanel>()?.Refresh(_enemyType, _enemyHp, _enemyMaxHp);
        }

        public override void OnExit()
        {
            Game.UIManager?.HidePanel<UI.Panels.UICombatPanel>();
        }

        /// <summary>Called by UI Attack button.</summary>
        public void OnPlayerAttack(int playerDamage = 10)
        {
            if (!_inCombat) return;

            _enemyHp -= playerDamage;
            Debug.Log($"[Combat] Player attacks for {playerDamage}. Enemy HP: {_enemyHp}/{_enemyMaxHp}");

            if (_enemyHp <= 0)
            {
                OnCombatVictory();
                return;
            }

            // Enemy counter-attack
            Game.Player.ApplyDamage(_enemyAttack);
            Debug.Log($"[Combat] Enemy attacks for {_enemyAttack}. Player HP: {Game.Player.Stats.CurrentHp}");

            Game.UIManager?.GetPanel<UI.Panels.UICombatPanel>()?.Refresh(_enemyType, _enemyHp, _enemyMaxHp);

            if (!Game.Player.IsAlive)
                OnCombatDefeat();
        }

        private void OnCombatVictory()
        {
            _inCombat = false;
            Debug.Log("[Combat] Victory!");

            // Reward: give a random place card
            AwardCard();

            if (_enemyType == PlaceType.Boss)
            {
                Game.EndRun(victory: true);
                return;
            }

            if (Game.Navigator.IsAtLeaf)
                Game.EndRun(victory: true);
            else
                Game.StateMachine.TransitionTo<StateMapNavigation>();
        }

        private void OnCombatDefeat()
        {
            _inCombat = false;
            Debug.Log("[Combat] Defeat!");
            Game.EndRun(victory: false);
        }

        private void AwardCard()
        {
            // Award a gold reward for now; card rewards can be added here
            int goldReward = _enemyType switch
            {
                PlaceType.Elite => 20,
                PlaceType.Boss  => 50,
                _               => 10
            };
            Game.Player.ModifyGold(goldReward);
            Debug.Log($"[Combat] Awarded {goldReward} gold.");
        }
    }
}
