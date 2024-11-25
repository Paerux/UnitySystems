using System.Collections.Generic;
using Paerux.Persistence;
using UnityEngine;

namespace Paerux.Achievements
{
    [System.Serializable]
    public class AchievementData
    {
        public string id;
        public bool unlocked;
        public int currentProgress;

        public AchievementData(string id, bool unlocked, int currentProgress)
        {
            this.id = id;
            this.unlocked = unlocked;
            this.currentProgress = currentProgress;
        }
    }

    public class AchievementListData : ISaveData
    {
        public AchievementData[] achievements;
    }
}