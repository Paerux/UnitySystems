using System;
using System.Linq;
using Paerux.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Paerux.Persistence
{
    public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
    {
        public GameData gameData;
        IDataService dataService;

        protected override void Awake()
        {
            base.Awake();
            dataService = new JSONDataService();
        }
        public void NewGame()
        {
            gameData = new GameData { CurrentLevelName = "SaveLoad Demo" };
            SceneManager.LoadScene(gameData.CurrentLevelName);
        }
        public void SaveGame() => dataService.Save("test-save.json", gameData);
        public void LoadGame(string dataName)
        {
            gameData = (GameData)dataService.Load(dataName);
            if (string.IsNullOrWhiteSpace(gameData.CurrentLevelName))
            {
                gameData.CurrentLevelName = "SaveLoad Demo";
            }
            SceneManager.LoadScene(gameData.CurrentLevelName);
        }

        public void DeleteGame(string dataName) => dataService.Delete(dataName);
        public void ReloadGame() => LoadGame("test-save.json");

        private void OnSaveEvent(SaveEvent.SaveEventType saveEventType, SaveEventArgs saveEventArgs)
        {
            if (saveEventType == SaveEvent.SaveEventType.Save)
            {
                dataService.Save("test-save.json", saveEventArgs.SaveData, saveEventArgs.Encrypt);
            }
            else if (saveEventType == SaveEvent.SaveEventType.Load)
            {
                gameData = (GameData)dataService.Load("test-save.json", saveEventArgs.Encrypt);
            }
            else if (saveEventType == SaveEvent.SaveEventType.Delete)
            {
                dataService.Delete("test-save.json");
            }
        }

        private void OnEnable()
        {
            SaveEvent.Register(OnSaveEvent);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDisable()
        {
            SaveEvent.Unregister(OnSaveEvent);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }



        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene loaded: " + scene.name);
            foreach (IBindable bindable in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IBindable>())
            {
                bindable.Bind(gameData);
            }
        }
    }
}
