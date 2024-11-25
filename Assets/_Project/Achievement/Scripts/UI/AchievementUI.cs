using System.Collections;
using UnityEngine;
using Paerux.Localization;
using Paerux.UI;
using System.Collections.Generic;

namespace Paerux.Achievements
{
    public class AchievementUI : MonoBehaviour
    {
        public UIAchievementItem achievementItemPrefab;
        public AchievementList achievementList;

        public int maxShownAchievements = 3;
        public float achievementDisplayTime = 3f;
        public float achievementFadeTime = 0.2f;

        private Queue<Achievement> achievementQueue = new Queue<Achievement>();
        private int currentlyShownAchievements = 0;

        public IEnumerator DisplayAchievement(Achievement achievement)
        {
            if (achievementItemPrefab == null)
            {
                Debug.LogError("Achievement Item Prefab is not set.");
                yield break;
            }

            var achievementItem = Instantiate(achievementItemPrefab, transform);

            achievementItem.title.text = LocalizationSystem.GetLocalizedValue(achievement.titleKey, achievement.titleKey);
            achievementItem.description.text = LocalizationSystem.GetLocalizedValue(achievement.descriptionKey, achievement.descriptionKey);
            achievementItem.icon.sprite = achievement.unlockedSprite;

            if (achievement.achievementType == AchievementType.Progress)
            {
                achievementItem.progressBar.gameObject.SetActive(true);
                achievementItem.progressBar.SetProgress(achievement.currentProgress, achievement.requiredProgress);
            }
            else
            {
                achievementItem.progressBar.gameObject.SetActive(false);
            }

            CanvasGroup canvasGroup = achievementItem.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                StartCoroutine(UIHelpers.FadeCanvasGroup(canvasGroup, fadeSpeed: 1f / achievementFadeTime, startAlpha: 0f, endAlpha: 1f));
                yield return new WaitForSeconds(achievementDisplayTime);
                StartCoroutine(UIHelpers.FadeCanvasGroup(canvasGroup, fadeSpeed: 1f / achievementFadeTime, startAlpha: 1f, endAlpha: 0f));
                yield return new WaitForSeconds(achievementFadeTime + 0.5f);
                Destroy(achievementItem.gameObject);
                currentlyShownAchievements--;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                var achievement = achievementList.achievements[0];
                AchievementEvent.Trigger(AchievementEvent.AchievementEventType.AchievementUnlocked, new AchievementEventArgs(achievement));
            }

            if (achievementQueue.Count > 0 && currentlyShownAchievements < maxShownAchievements)
            {
                StartCoroutine(DisplayAchievement(achievementQueue.Dequeue()));
                currentlyShownAchievements++;
            }
        }

        private void OnEnable()
        {
            AchievementEvent.Register(OnAchievementEvent);
        }

        private void OnDisable()
        {
            AchievementEvent.Unregister(OnAchievementEvent);
        }

        private void OnAchievementEvent(AchievementEvent.AchievementEventType achievementEventType, AchievementEventArgs achievementEventArgs)
        {
            switch (achievementEventType)
            {
                case AchievementEvent.AchievementEventType.AchievementUnlocked:
                    achievementQueue.Enqueue(achievementEventArgs.achievement);
                    break;
                case AchievementEvent.AchievementEventType.AchievementProgressUpdated:
                    // Show a progress update
                    break;
            }
        }
    }
}