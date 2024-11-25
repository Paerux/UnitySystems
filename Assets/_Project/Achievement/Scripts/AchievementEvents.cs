namespace Paerux.Achievements
{
    public struct AchievementEvent
    {
        public enum AchievementEventType
        {
            AchievementUnlocked,
            AchievementProgressUpdated
        }
        public AchievementEventType eventType;
        public AchievementEventArgs achievementEventArgs;

        public AchievementEvent(AchievementEventType eventType, AchievementEventArgs achievementEventArgs)
        {
            this.eventType = eventType;
            this.achievementEventArgs = achievementEventArgs;
        }

        public delegate void AchievementDelegate(AchievementEventType achievementEventType, AchievementEventArgs achievementEventArgs);
        public static event AchievementDelegate OnAchievementEvent;

        public static void Register(AchievementDelegate caller)
        {
            OnAchievementEvent += caller;
        }

        public static void Unregister(AchievementDelegate caller)
        {
            OnAchievementEvent -= caller;
        }

        public static void Trigger(AchievementEventType achievementEventType, AchievementEventArgs achievementEventArgs)
        {
            OnAchievementEvent?.Invoke(achievementEventType, achievementEventArgs);
        }
    }

    public struct AchievementEventArgs
    {
        public Achievement achievement;

        public AchievementEventArgs(Achievement achievement)
        {
            this.achievement = achievement;
        }
    }
}