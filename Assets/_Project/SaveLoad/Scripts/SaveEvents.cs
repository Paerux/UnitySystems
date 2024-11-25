namespace Paerux.Persistence
{
    public struct SaveEvent
    {
        public enum SaveEventType
        {
            Save,
            Load,
            Delete
        }
        public SaveEventType eventType;
        public SaveEventArgs saveEventArgs;

        public SaveEvent(SaveEventType eventType, SaveEventArgs saveEventArgs)
        {
            this.eventType = eventType;
            this.saveEventArgs = saveEventArgs;
        }

        public delegate void SaveDelegate(SaveEventType saveEventType, SaveEventArgs saveEventArgs);
        public static event SaveDelegate OnSaveEvent;

        public static void Register(SaveDelegate caller)
        {
            OnSaveEvent += caller;
        }

        public static void Unregister(SaveDelegate caller)
        {
            OnSaveEvent -= caller;
        }

        public static void Trigger(SaveEventType saveEventType, SaveEventArgs saveEventArgs)
        {
            OnSaveEvent?.Invoke(saveEventType, saveEventArgs);
        }
    }

    public struct SaveEventArgs
    {
        public ISaveData SaveData;
        public bool Encrypt;

        public SaveEventArgs(ISaveData saveData, bool encrypt = false)
        {
            Encrypt = encrypt;
            SaveData = saveData;
        }
    }
}