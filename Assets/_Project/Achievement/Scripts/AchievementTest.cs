using System;
using UnityEngine;

namespace Paerux.Achievements
{
    public class AchievementTest : MonoBehaviour
    {
        public AchievementList achievementList;

        private void Awake()
        {
            AchievementSystem.InitializeAchievementSystem(achievementList);
            AchievementSystem.LoadAchievementProgresses();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var randomAchievement = AchievementSystem.achievements[UnityEngine.Random.Range(0, AchievementSystem.achievements.Count)];
                Debug.Log("Adding progress to: " + randomAchievement.id);
                AchievementSystem.AddProgress(randomAchievement.id, 1);
            }
        }

        private void OnEnable()
        {
            AchievementEvent.Register(OnAchievementEvent);
        }

        private void OnAchievementEvent(AchievementEvent.AchievementEventType achievementEventType, AchievementEventArgs achievementEventArgs)
        {
            switch (achievementEventType)
            {
                case AchievementEvent.AchievementEventType.AchievementUnlocked:
                    Debug.Log("Achievement Unlocked: " + achievementEventArgs.achievement.id);
                    break;
                case AchievementEvent.AchievementEventType.AchievementProgressUpdated:
                    Debug.Log("Achievement Progress Updated: " + achievementEventArgs.achievement.id);
                    break;
            }
        }

        private void OnDisable()
        {
            AchievementEvent.Unregister(OnAchievementEvent);
        }
    }

}