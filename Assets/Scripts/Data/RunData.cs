using System;
using System.Collections.Generic;

namespace RogueCard.Data
{
    /// <summary>
    /// Serializable snapshot of a run's state for save/load.
    /// </summary>
    [Serializable]
    public class RunData
    {
        public int seed;
        public int currentHp;
        public int maxHp;
        public int gold;
        public int currentDepth;
        public string currentNodeId;
        public List<string> handCardIds = new();
        public bool isVictory;
        public bool isGameOver;

        public DateTime runStartTime;
        public DateTime runEndTime;
    }
}
