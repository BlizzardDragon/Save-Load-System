using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveLoad
{
    public sealed class GameRepositoryBinary : IGameRepository
    {
        private Dictionary<string, string> _gameState = new();
        private const string SAVE_PATH = "/Save.gamesave";
        private readonly string _filePath;
        private ByteEncryptor _byteEncryptor = new();

        public GameRepositoryBinary()
        {
            _filePath = Application.persistentDataPath + SAVE_PATH;
        }


        public bool HasData()
        {
            return _gameState.Count > 0;
        }

        public void SaveState()
        {
            byte[] bytes = ConvertObjectToBytes(_gameState);
            byte[] encryptedBytes = _byteEncryptor.EncryptBytes(bytes);
            SaveBytes(encryptedBytes);
        }

        public void LoadState()
        {
            if (File.Exists(_filePath))
            {
                byte[] bytes = LoadBytes();
                byte[] decryptedBytes = _byteEncryptor.DecryptBytes(bytes);
                _gameState = ConvertBytesToObject<Dictionary<string, string>>(decryptedBytes);
            }
            else
            {
                _gameState = new();
            }
        }

        public T GetData<T>()
        {
            string serializedData = _gameState[typeof(T).Name];
            return JsonConvert.DeserializeObject<T>(serializedData);
        }

        public bool TryGetData<T>(out T value)
        {
            if (_gameState.TryGetValue(typeof(T).Name, out var serializedData))
            {
                value = JsonConvert.DeserializeObject<T>(serializedData);
                return true;
            }

            value = default;
            return false;
        }

        public void SetData<T>(T value)
        {
            string serializedData = JsonConvert.SerializeObject(value);
            _gameState[typeof(T).Name] = serializedData;
        }

        private byte[] ConvertObjectToBytes<T>(T obj)
        {
            string jsonData = JsonConvert.SerializeObject(obj); // Преобразование объекта в JSON строку
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData); // Преобразование строки в байты

            return bytes;
        }

        private T ConvertBytesToObject<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                Debug.LogWarning("Data bytes are empty.");
                return default; // Возвращаем значение по умолчанию для типа T
            }

            string jsonData = System.Text.Encoding.UTF8.GetString(bytes); // Преобразование байтов в строку JSON
            T obj = JsonConvert.DeserializeObject<T>(jsonData); // Преобразование JSON строки в объект
            
            return obj;
        }

        private void SaveBytes(byte[] bytes)
        {
            using (FileStream fileStream = new FileStream(_filePath, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(fileStream))
                {
                    writer.Write(bytes); // Записываем массив байтов в файл
                }
            }
        }

        private byte[] LoadBytes()
        {
            byte[] bytes;

            if (!File.Exists(_filePath))
            {
                bytes = new byte[0];
            }
            else
            {
                using (FileStream fileStream = new FileStream(_filePath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(fileStream))
                    {
                        bytes = reader.ReadBytes((int)fileStream.Length); // Чтение всех доступных байтов из файла
                    }
                }
            }

            return bytes;
        }

        public void ClearData() => File.Delete(_filePath);
    }
}