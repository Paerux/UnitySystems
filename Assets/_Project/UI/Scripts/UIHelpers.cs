using System.Collections;
using UnityEngine;

namespace Paerux.UI
{
    public static class UIHelpers
    {
        public static IEnumerator FadeCanvasGroup(CanvasGroup group, float fadeSpeed, float startAlpha, float endAlpha)
        {
            group.alpha = startAlpha;
            group.gameObject.SetActive(true);
            group.interactable = false;

            if (startAlpha <= endAlpha) // Fade in
            {
                while (group.alpha <= endAlpha)
                {
                    group.alpha += Time.unscaledDeltaTime * fadeSpeed;
                    if (group.alpha >= endAlpha)
                    {
                        break;
                    }
                    yield return null;
                }
                group.interactable = true;

                yield return null;
            }
            else // Fade out
            {
                while (group.alpha > endAlpha)
                {
                    group.alpha -= Time.unscaledDeltaTime * fadeSpeed;
                    if (group.alpha <= endAlpha)
                    {
                        break;
                    }
                    yield return null;
                }
                yield return null;
            }

            group.alpha = endAlpha;
            if (endAlpha <= 0f)
            {
                group.gameObject.SetActive(false);
            }
        }
    }
}