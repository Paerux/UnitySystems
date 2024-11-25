using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Paerux.Localization
{
    public static class LocalizationSystem
    {
        private static Dictionary<string, Dictionary<string, string>> allLanguages = new Dictionary<string, Dictionary<string, string>>();
        private static bool isInitialized = false;
        private static string currentLanguage = "English";
        private const string PlayerPrefsKey = "Language";

        public static void Initialize()
        {
            LoadAllLanguages();
            string savedLanguage = PlayerPrefs.GetString(PlayerPrefsKey, "English");
            if (!SetLanguage(savedLanguage))
            {
                Debug.LogWarning("Language not found: " + savedLanguage + ". Falling back to English.");
                SetLanguage("English");
            }
            isInitialized = true;
        }

        private static void LoadAllLanguages()
        {
            allLanguages.Clear();
            TextAsset[] files = Resources.LoadAll<TextAsset>("Localization");
            foreach (TextAsset file in files)
            {
                string language = Path.GetFileNameWithoutExtension(file.name);
                allLanguages[language] = new Dictionary<string, string>();
                string[] lines = file.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2 && !allLanguages[language].ContainsKey(parts[0].Trim()))
                    {
                        allLanguages[language].Add(parts[0].Trim(), parts[1].Trim());
                    }
                }
            }
            Debug.Log("Languages loaded: " + string.Join(", ", allLanguages.Keys));
        }

        public static bool SetLanguage(string language)
        {
            if (allLanguages.ContainsKey(language))
            {
                currentLanguage = language;
                PlayerPrefs.SetString(PlayerPrefsKey, language);
                PlayerPrefs.Save();
                Debug.Log("Language set to: " + language);
                return true;
            }

            Debug.LogWarning("Language not found: " + language);
            return false;
        }

        public static string GetLocalizedValue(string key, string defaultValue = "")
        {
            if (!isInitialized)
            {
                Debug.LogWarning("Localization not initialized. Initializing");
                Initialize();
            }
            if (allLanguages[currentLanguage].ContainsKey(key))
            {
                return allLanguages[currentLanguage][key];
            }
            else
            {
                Debug.LogWarning("Key not found: " + key);
                return defaultValue;
            }
        }

        public static string GetCurrentLanguage() => currentLanguage;
        public static List<string> GetAvailableLanguages() => new List<string>(allLanguages.Keys);
    }
}