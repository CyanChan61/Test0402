using System.IO;
using UnityEngine;

namespace RogueCard.Data
{
    public class SaveLoadManager
    {
        private const string SaveFileName = "run_save.json";

        private static string SavePath =>
            Path.Combine(Application.persistentDataPath, SaveFileName);

        public void Save(RunData data)
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"[SaveLoadManager] Saved run to {SavePath}");
        }

        public RunData Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("[SaveLoadManager] No save file found.");
                return null;
            }

            string json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<RunData>(json);
            Debug.Log("[SaveLoadManager] Loaded run data.");
            return data;
        }

        public bool HasSave() => File.Exists(SavePath);

        public void DeleteSave()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
    }
}
