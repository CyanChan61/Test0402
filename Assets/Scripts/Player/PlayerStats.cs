using System;
using UnityEngine;

namespace RogueCard.Player
{
    [Serializable]
    public class PlayerStats
    {
        [field: SerializeField] public int MaxHp { get; private set; }
        [field: SerializeField] public int CurrentHp { get; private set; }

        public event Action<int, int> OnHpChanged;  // (current, max)

        public PlayerStats(int maxHp)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public void ApplyDamage(int amount)
        {
            if (amount <= 0) return;
            CurrentHp = Mathf.Max(0, CurrentHp - amount);
            OnHpChanged?.Invoke(CurrentHp, MaxHp);
        }

        public void ApplyHeal(int amount)
        {
            if (amount <= 0) return;
            CurrentHp = Mathf.Min(MaxHp, CurrentHp + amount);
            OnHpChanged?.Invoke(CurrentHp, MaxHp);
        }

        public void SetMaxHp(int newMax, bool healToFull = false)
        {
            MaxHp = newMax;
            if (healToFull) CurrentHp = MaxHp;
            CurrentHp = Mathf.Min(CurrentHp, MaxHp);
            OnHpChanged?.Invoke(CurrentHp, MaxHp);
        }

        public bool IsAlive => CurrentHp > 0;
        public float HpPercent => MaxHp > 0 ? (float)CurrentHp / MaxHp : 0f;
    }
}
