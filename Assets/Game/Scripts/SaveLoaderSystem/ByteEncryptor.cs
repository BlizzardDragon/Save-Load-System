using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace SaveLoad
{
    // Длинна смещения ключей в пределах диапазона может быть любой.
    // При выходе ключа за границы данных, данные будут автоматически сгенерированы.
    public class ByteEncryptor
    {
        private RandomNumberGenerator _randomGenerator = RandomNumberGenerator.Create();

        private const int LENGTH_ENCRYPTION_KEY = 16;
        // Range: 1 - Infinity.
        private const int KEY_OFFSET_1 = 6;
        // Range: 1 - 255.
        private const int KEY_OFFSET_2 = 15;
        private const int SALT_LENGTH = 34;
        private const string PLAYER_PREFS_KEY = "Key";


        private byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            _randomGenerator.GetBytes(randomBytes);
            return randomBytes;
        }

        private byte GetXORByte(byte[] data, int dataIndex, byte[] key, int keyIndex)
        {
            return (byte)(data[dataIndex] ^ key[keyIndex % key.Length]);
        }

        // Сохранение ключа на случай его утраты.
        private void SaveKeyToPlayerPrefs(byte[] encryptionKey)
        {
            string byteArrayString = Convert.ToBase64String(encryptionKey);
            PlayerPrefs.SetString(PLAYER_PREFS_KEY, byteArrayString);
            PlayerPrefs.Save();
        }

        public byte[] EncryptBytes(byte[] bytes)
        {
            Debug.Log($"Serialized data size before encrypt: <color=green>{bytes.Length}</color> bytes!");

            byte[] encryptionKey = GenerateRandomBytes(LENGTH_ENCRYPTION_KEY);
            SaveKeyToPlayerPrefs(encryptionKey);

            Debug.Log($"Saved Key: <color=black>{string.Join(" ", encryptionKey)}</color>");

            List<byte> encryptedList = new();

            // Добавление к данным примеси из случайных байтов и применение XOR с ключем.
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 2 == 0)
                {
                    encryptedList.Add(GenerateRandomBytes(1)[0]);
                }
                encryptedList.Add(GetXORByte(bytes, i, encryptionKey, i));
            }

            #region Write key to data
            byte[] salt = GenerateRandomBytes(SALT_LENGTH);

            // Множитель для распределения элементов ключа по массиву данных.
            float keyMultiplicity = encryptedList.Count / LENGTH_ENCRYPTION_KEY;

            // Расчет случайного смещения ключа в зависимости от последнего байта соли.
            float divider = byte.MaxValue / KEY_OFFSET_2;
            float randomNumberRange = Mathf.Min(byte.MaxValue, salt[SALT_LENGTH - 1] + divider);
            float linearRandomOffset = randomNumberRange / divider;

            // Расчет верхнего придела смещения для определения размера подписи без учета убывающей прогрессии.
            int maxLinearOffset = encryptionKey.Length * (KEY_OFFSET_1 + KEY_OFFSET_2);

            // Распределение ключа по массиву данных.
            int listIndex;
            for (int i = 0; i < encryptionKey.Length; i++)
            {
                // Расчет случайного смещения с добавлением прогрессии.
                float keyPosition = i * keyMultiplicity;
                float progressiveRandomOffset = linearRandomOffset * (keyPosition / encryptedList.Count);

                listIndex = (int)(keyPosition + KEY_OFFSET_1 + progressiveRandomOffset);

                // Добавление случайных байтов в конец данных в случае превышения длинны массива из-за смещения.
                // При текущих соотношениях длинны данных и констант, такого не должно происходить.
                while (listIndex >= encryptedList.Count)
                {
                    byte newByte = GenerateRandomBytes(1)[0];
                    encryptedList.Add(newByte);
                    maxLinearOffset--;
                    Debug.Log($"Added random byte <color=red>{newByte}</color> at index" +
                        $"<color=red>{encryptedList.Count - 1}</color> due to array length exceeded");
                }

                // Debug.Log($"encryptedList.Count = {encryptedList.Count}, listIndex = {listIndex}");
                encryptedList.Insert(listIndex, encryptionKey[i]);
            }

            // Добавление к данным соли и подписи. 
            byte[] signature = GenerateRandomBytes(maxLinearOffset);
            encryptedList.InsertRange(0, salt);
            encryptedList.AddRange(signature);
            #endregion

            byte[] encryptedBytes = encryptedList.ToArray();
            Debug.Log($"Serialized data size after encrypt: <color=red>{encryptedBytes.Length}</color> bytes!");

            return encryptedBytes;
        }

        public byte[] DecryptBytes(byte[] bytes)
        {
            Debug.Log($"Serialized data size before decrypt: <color=red>{bytes.Length}</color> bytes!");

            #region Extract key from data
            List<byte> byteList = new List<byte>(bytes);
            byte[] encryptionKey = new byte[LENGTH_ENCRYPTION_KEY];

            // Расчет случайного смещения ключа в зависимости от последнего байта соли.
            float divider = byte.MaxValue / KEY_OFFSET_2;
            float randomNumberRange = Mathf.Min(byte.MaxValue, byteList[SALT_LENGTH - 1] + divider);
            float linearRandomOffset = randomNumberRange / divider;

            // Расчет верхнего придела смещения для определения размера подписи без учета убывающей прогрессии.
            int maxLinearOffset = encryptionKey.Length * (KEY_OFFSET_1 + KEY_OFFSET_2);

            // Восстановление множителя для распределения элементов ключа в данных.
            float keyMultiplicity =
                (byteList.Count - LENGTH_ENCRYPTION_KEY - SALT_LENGTH - maxLinearOffset) / LENGTH_ENCRYPTION_KEY;

            // Очистка данных от соли.
            byteList.RemoveRange(0, SALT_LENGTH);

            // Восстановление ключа и удаление его из данных.
            int listIndex;
            for (int i = encryptionKey.Length - 1; i >= 0; i--)
            {
                // Расчет случайного смещения с добавлением прогрессии.
                float keyPosition = i * keyMultiplicity;
                float progressiveRandomOffset = linearRandomOffset * (keyPosition / (byteList.Count - maxLinearOffset));

                listIndex = (int)(keyPosition + KEY_OFFSET_1 + progressiveRandomOffset);
                encryptionKey[i] = byteList[listIndex];

                byteList.RemoveAt(listIndex);
            }

            // Очистка данных от подписи. 
            // Выполняется после извлечения ключа, так как ключ частично может находиться в подписи.
            byteList.RemoveRange(byteList.Count - maxLinearOffset, maxLinearOffset);

            Debug.Log($"Loaded key: <color=black>{string.Join(" ", encryptionKey)}</color>");
            #endregion

            byte[] bytesArray = byteList.ToArray();
            List<byte> decryptedList = new();
            int index = 0;

            // Отделение данных от примеси и XOR дешифровка по ключу.
            for (int i = 0; i < bytesArray.Length; i++)
            {
                if (i % 3 != 0)
                {
                    decryptedList.Add(GetXORByte(bytesArray, i, encryptionKey, index));
                    index++;
                }
            }

            byte[] decryptedBytes = decryptedList.ToArray();
            Debug.Log($"Serialized data size after decrypt: <color=green>{decryptedBytes.Length}</color> bytes!");

            return decryptedBytes;
        }
    }
}