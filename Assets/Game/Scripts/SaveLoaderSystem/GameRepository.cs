using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveLoad
{
    public sealed class GameRepository : IGameRepository
    {
        private Dictionary<string, string> _gameState = new();

        private const string KEY_GAME_STATE = "GameState";


        public bool HasData()
        {
            return _gameState.Count > 0;
        }

        public void SaveState()
        {
            string serializedState = JsonConvert.SerializeObject(_gameState);
            PlayerPrefs.SetString(KEY_GAME_STATE, serializedState);
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

        public void LoadState()
        {
            if (PlayerPrefs.HasKey(KEY_GAME_STATE))
            {
                string serializedState = PlayerPrefs.GetString(KEY_GAME_STATE);
                _gameState = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedState);
            }
            else
            {
                _gameState = new();
            }
        }

        public void ClearData() => PlayerPrefs.DeleteKey(KEY_GAME_STATE);
    }
}