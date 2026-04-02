using UnityEngine;

namespace RogueCard.Data
{
    [CreateAssetMenu(menuName = "RogueCard/RunConfig", fileName = "DefaultRunConfig")]
    public class RunConfig : ScriptableObject
    {
        [Header("Map")]
        [Tooltip("Max depth of the binary tree (number of levels below root)")]
        public int treeMaxDepth = 8;

        [Header("Player")]
        public int startingHp = 80;
        public int startingGold = 0;
        public int startingHandSize = 3;
        public int maxHandSize = 6;

        [Header("Combat")]
        public int baseEnemyHp = 20;
        public int baseEnemyAttack = 5;
        public float eliteHpMultiplier = 2f;
        public float eliteAttackMultiplier = 1.5f;
        public float bossHpMultiplier = 4f;
        public float bossAttackMultiplier = 2f;

        [Header("Rest")]
        [Tooltip("HP restored when resting")]
        public int restHealAmount = 15;

        [Header("Treasure")]
        public int treasureGoldMin = 10;
        public int treasureGoldMax = 30;
    }
}
