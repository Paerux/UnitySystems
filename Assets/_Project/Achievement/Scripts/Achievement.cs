using UnityEngine;
using Paerux.Persistence;
namespace Paerux.Achievements
{
    public enum AchievementType
    {
        Single,
        Progress
    }
    [System.Serializable]
    public class Achievement
    {
        [Header("Achievement Data")]
        public string id;
        public AchievementType achievementType;
        public string titleKey;
        public string descriptionKey;
        public bool hidden;
        public bool unlocked;

        [Header("Image and Sounds")]
        public Sprite lockedSprite;
        public Sprite unlockedSprite;
        public AudioClip unlockedSound;

        [Header("Progress Data")]
        public int currentProgress;
        public int requiredProgress;

        public void UnlockAchievement()
        {
            if (unlocked)
            {
                Debug.LogWarning("Achievement already unlocked");
                return;
            }
            unlocked = true;
            AchievementEvent.Trigger(AchievementEvent.AchievementEventType.AchievementUnlocked, new AchievementEventArgs(this));
        }

        public void SetProgress(int progress)
        {
            if (unlocked)
            {
                Debug.LogWarning("Achievement already unlocked");
                return;
            }
            currentProgress = progress;
            if (currentProgress >= requiredProgress)
            {
                UnlockAchievement();
            }
            else
            {
                AchievementEvent.Trigger(AchievementEvent.AchievementEventType.AchievementProgressUpdated, new AchievementEventArgs(this));
            }
        }

        public void AddProgress(int progress)
        {
            if (unlocked)
            {
                Debug.LogWarning("Achievement already unlocked");
                return;
            }
            currentProgress += progress;
            if (currentProgress >= requiredProgress)
            {
                UnlockAchievement();
            }
            else
            {
                AchievementEvent.Trigger(AchievementEvent.AchievementEventType.AchievementProgressUpdated, new AchievementEventArgs(this));
            }
        }

        public Achievement CreateCopy()
        {
            Achievement copy = new Achievement();
            copy = JsonUtility.FromJson<Achievement>(JsonUtility.ToJson(this));
            return copy;
        }
    }
}