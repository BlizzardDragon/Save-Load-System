using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SaveLoadManager : MonoBehaviour
    {
        private GameRepositoryBinary _repositoryBinary;
        private ISaveLoader[] _saveLoaders;


        [Inject]
        public void Construct(ISaveLoader[] saveLoaders, GameRepositoryBinary repository)
        {
            _saveLoaders = saveLoaders;
            _repositoryBinary = repository;
        }

        [Button]
        public void Load()
        {
            _repositoryBinary.LoadState();

            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.LoadData(_repositoryBinary);
            }
        }

        [Button]
        public void Save()
        {
            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.SaveData(_repositoryBinary);
            }

            _repositoryBinary.SaveState();
        }

        [Button]
        public void ClearData() => _repositoryBinary.ClearData();


        // //TODO: TIMER

        // private void OnApplicationFocus(bool focusStatus)
        // {
        //     if (!_repository.HasData()) return;

        //     if (!focusStatus)
        //     {
        //         Save();
        //     }
        // }

        // private void OnApplicationPause(bool pauseStatus)
        // {
        //     if (!_repository.HasData()) return;

        //     if (pauseStatus)
        //     {
        //         Save();
        //     }
        // }

        // private void OnApplicationQuit()
        // {
        //     if (!_repository.HasData()) return;

        //     Save();
        // }
    }
}
