using System.Collections.Generic;
using UnityEngine;

namespace Paerux.Achievements
{
    [CreateAssetMenu(fileName = "AchievementList", menuName = "Achievement System/Achievement List")]
    public class AchievementList : ScriptableObject
    {
        public List<Achievement> achievements;
    }
}