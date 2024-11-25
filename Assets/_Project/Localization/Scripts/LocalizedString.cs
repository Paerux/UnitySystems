namespace Paerux.Localization
{
    [System.Serializable]
    public class LocalizedString
    {
        public string key;
        public string defaultValue;
        public LocalizedString(string key, string defaultValue = "")
        {
            this.key = key;
            this.defaultValue = defaultValue;
        }

        public string GetValue() => LocalizationSystem.GetLocalizedValue(key, defaultValue);
        public override string ToString() => GetValue();
    }
}