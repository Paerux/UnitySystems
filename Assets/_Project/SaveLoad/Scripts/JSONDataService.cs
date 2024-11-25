using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;


namespace Paerux.Persistence
{
    public class JSONDataService : IDataService
    {
        private readonly string baseSavePath = Path.Combine(Application.persistentDataPath, "Saves");
        private readonly byte[] encryptionKey = Encoding.UTF8.GetBytes("RandomGeneratedKeyxD");

        private void CheckFileConditions()
        {
            if (!Directory.Exists(baseSavePath))
            {
                Directory.CreateDirectory(baseSavePath);
            }
        }
        public bool Save(string relativePath, ISaveData data, bool encrypt = false)
        {
            CheckFileConditions();
            string path = Path.Combine(baseSavePath, relativePath);
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            try
            {
                var container = new SaveDataContainer
                {
                    Type = data.GetType().AssemblyQualifiedName,
                    JsonData = data
                };

                string json = JsonConvert.SerializeObject(container, Formatting.Indented);
                if (encrypt)
                    json = Encrypt(json);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving data to {path}: {e.Message}");
                return false;
            }
        }
        public ISaveData Load(string relativePath, bool decrypt = false)
        {
            CheckFileConditions();
            string path = Path.Combine(baseSavePath, relativePath);
            try
            {
                string json = File.ReadAllText(path);
                if (decrypt)
                    json = Decrypt(json);

                var container = JsonConvert.DeserializeObject<SaveDataContainer>(json);
                var type = Type.GetType(container.Type);
                if (type == null)
                {
                    Debug.LogError($"Error loading data from {path}: Type not found");
                    return null;
                }

                var jsonData = JsonConvert.DeserializeObject(container.JsonData.ToString(), type);
                return (ISaveData)jsonData;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading data from {path}: {e.Message}");
                return null;
            }
        }
        public bool Delete(string relativePath)
        {
            string path = Path.Combine(baseSavePath, relativePath);
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deleting data from {path}: {e.Message}");
                return false;
            }
        }

        public IEnumerable<ISaveData> ListSaves()
        {
            try
            {
                if (!Directory.Exists(Application.persistentDataPath))
                {
                    return Enumerable.Empty<ISaveData>();
                }

                var files = Directory.GetFiles(Application.persistentDataPath, "*.json");
                var saves = new List<ISaveData>();

                foreach (var file in files)
                {
                    string json = File.ReadAllText(file);
                    saves.Add(JsonUtility.FromJson<ISaveData>(json));
                }

                return saves;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error listing saves: {e.Message}");
                return Enumerable.Empty<ISaveData>();
            }
        }

        private string Encrypt(string json)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.GenerateIV();
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(json);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);
                    return Convert.ToBase64String(result);
                }
            }
        }

        private string Decrypt(string encryptedJson)
        {
            byte[] encryptedBytesWithIV = Convert.FromBase64String(encryptedJson);
            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] encryptedBytes = new byte[encryptedBytesWithIV.Length - iv.Length];

                Buffer.BlockCopy(encryptedBytesWithIV, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytesWithIV, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }

    [Serializable]
    public class SaveDataContainer
    {
        public string Type;
        public object JsonData;
    }
}
