using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Paerux.UI
{
    public class ProgressBar : MonoBehaviour
    {
        public Image fill;
        public TextMeshProUGUI text;

        public void SetProgress(int currentProgress, int requiredProgress)
        {
            if (requiredProgress == 0)
            {
                Debug.LogError("Required progress is 0.");
                return;
            }

            var progress = (float)currentProgress / requiredProgress;
            fill.fillAmount = progress;
            text.text = $"{currentProgress}/{requiredProgress}";
        }
    }
}