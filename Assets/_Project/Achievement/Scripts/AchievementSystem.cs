using System.Collections.Generic;
using Paerux.Persistence;
using UnityEditor;
using UnityEngine;

namespace Paerux.Achievements
{
    public static class AchievementSystem
    {
        public static List<Achievement> achievements;
        static IDataService dataService;

        public static void InitializeAchievementSystem(AchievementList achievementList)
        {
            if (achievementList == null) return;
            achievements = new List<Achievement>();
            dataService = new JSONDataService();
            foreach (var achievement in achievementList.achievements)
            {
                achievements.Add(achievement.CreateCopy());
            }
        }

        public static void LoadAchievementProgresses()
        {
            AchievementListData achievementListData = dataService.Load("account-data.json") as AchievementListData;
            if (achievementListData == null) return;
            foreach (var achievementData in achievementListData.achievements)
            {
                var achievement = achievements.Find(a => a.id == achievementData.id);
                if (achievement != null)
                {
                    achievement.unlocked = achievementData.unlocked;
                    achievement.currentProgress = achievementData.currentProgress;
                }
            }
        }

        public static void SaveAchievementProgresses()
        {
            AchievementListData achievementListData = new AchievementListData();
            achievementListData.achievements = new AchievementData[achievements.Count];
            for (int i = 0; i < achievements.Count; i++)
            {
                AchievementData achievementData = new AchievementData(achievements[i].id, achievements[i].unlocked, achievements[i].currentProgress);
                achievementListData.achievements[i] = achievementData;
            }
            dataService.Save("account-data.json", achievementListData);
        }


        [MenuItem("Achievement System/Reset All Achievements")]
        public static void ResetAllAchievements()
        {
            foreach (var achievement in achievements)
            {
                achievement.unlocked = false;
                achievement.currentProgress = 0;
            }
            SaveAchievementProgresses();
            Debug.Log("All achievements reset");
        }

        public static void SetProgress(string achievementId, int progress)
        {
            var achievement = achievements.Find(a => a.id == achievementId);
            if (achievement != null)
            {
                achievement.SetProgress(progress);
                SaveAchievementProgresses();
            }
        }

        public static void AddProgress(string achievementId, int progress)
        {
            var achievement = achievements.Find(a => a.id == achievementId);
            if (achievement != null)
            {
                achievement.AddProgress(progress);
                SaveAchievementProgresses();
            }
        }

        public static void UnlockAchievement(string achievementId)
        {
            var achievement = achievements.Find(a => a.id == achievementId);
            if (achievement != null)
            {
                achievement.UnlockAchievement();
                SaveAchievementProgresses();
            }
        }

    }
}