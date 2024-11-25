using UnityEditor;
using UnityEngine;
namespace Paerux.Persistence.Editor
{
    [CustomEditor(typeof(SaveLoadManager))]
    public class SaveLoadManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SaveLoadManager saveLoadManager = (SaveLoadManager)target;
            DrawDefaultInspector();
            if (GUILayout.Button("New Game"))
            {
                saveLoadManager.NewGame();
            }
            if (GUILayout.Button("Save Game"))
            {
                SaveEvent.Trigger(SaveEvent.SaveEventType.Save, new SaveEventArgs(saveLoadManager.gameData, false));
            }
            if (GUILayout.Button("Load Game"))
            {
                SaveEvent.Trigger(SaveEvent.SaveEventType.Load, new SaveEventArgs(null, false));
            }
            if (GUILayout.Button("Delete Game"))
            {
                SaveEvent.Trigger(SaveEvent.SaveEventType.Delete, new SaveEventArgs(null));
            }
            if (GUILayout.Button("Reload Game"))
            {
                saveLoadManager.ReloadGame();
            }
        }
    }
}
