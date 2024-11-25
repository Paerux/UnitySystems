using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace Paerux.Localization
{
    public class LocalizationTest : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TMP_Dropdown dropdown;

        private void Start()
        {
            LocalizationSystem.Initialize();
            dropdown.ClearOptions();
            dropdown.AddOptions(LocalizationSystem.GetAvailableLanguages());
            string currentLanguage = LocalizationSystem.GetCurrentLanguage();
            int index = LocalizationSystem.GetAvailableLanguages().IndexOf(currentLanguage);
            if (index >= 0)
            {
                dropdown.value = index;
            }
            text.text = new LocalizedString("test").ToString();
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDropdownValueChanged(int arg0)
        {
            LocalizationSystem.SetLanguage(dropdown.options[arg0].text);
            text.text = new LocalizedString("test").ToString();
        }
    }
}
