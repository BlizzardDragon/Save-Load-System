using System.Collections.Generic;
using UnityEngine;
using Zenject;
using FrameworkUnity.OOP.Interfaces.Listeners;

namespace FrameworkUnity.OOP.Zenject
{
    public sealed class GameManagerContext
    {
        private readonly List<IGameListener> _listeners = new();
        private readonly List<IUpdateGameListener> _updateListeners = new();
        private readonly List<IFixedUpdateGameListener> _fixedUpdateListeners = new();
        private readonly List<ILateUpdateGameListener> _lateUpdateListeners = new();
        private readonly DiContainer _container;

        public GameManagerContext(DiContainer container)
        {
            _container = container;

            foreach (var listener in _container.Resolve<IEnumerable<IGameListener>>())
            {
                AddListener(listener);
            }
        }


        public void AddListener(IGameListener listener)
        {
            if (listener == null) return;

            _listeners.Add(listener);

            if (listener is IUpdateGameListener updateListener)
            {
                _updateListeners.Add(updateListener);
            }

            if (listener is IFixedUpdateGameListener fixedUpdateListener)
            {
                _fixedUpdateListeners.Add(fixedUpdateListener);
            }

            if (listener is ILateUpdateGameListener lateUpdateListener)
            {
                _lateUpdateListeners.Add(lateUpdateListener);
            }
        }

        public void RemoveListener(IGameListener listener)
        {
            if (listener == null) return;

            _listeners.Remove(listener);

            if (listener is IUpdateGameListener updateListener)
            {
                _updateListeners.Remove(updateListener);
            }

            if (listener is IFixedUpdateGameListener fixedUpdateListener)
            {
                _fixedUpdateListeners.Remove(fixedUpdateListener);
            }

            if (listener is ILateUpdateGameListener lateUpdateListener)
            {
                _lateUpdateListeners.Remove(lateUpdateListener);
            }
        }

        public void OnUpdate()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < _updateListeners.Count; i++)
            {
                _updateListeners[i].OnUpdate(deltaTime);
            }
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            for (int i = 0; i < _fixedUpdateListeners.Count; i++)
            {
                _fixedUpdateListeners[i].OnFixedUpdate(fixedDeltaTime);
            }
        }

        public void OnLateUpdate()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < _lateUpdateListeners.Count; i++)
            {
                _lateUpdateListeners[i].OnLateUpdate(deltaTime);
            }
        }

        internal void InitGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IInitGameListener initListener)
                {
                    initListener.OnInitGame();
                }
            }
        }

        internal void DeInitGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IDeInitGameListener deInitListener)
                {
                    deInitListener.OnDeInitGame();
                }
            }
        }

        public void PrepareGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IPrepareGameListener prepareListener)
                {
                    prepareListener.OnPrepareGame();
                }
            }
        }

        public void StartGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IStartGameListener startListener)
                {
                    startListener.OnStartGame();
                }
            }
        }

        public void PauseGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IPauseGameListener pauseListener)
                {
                    pauseListener.OnPauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IResumeGameListener resumeListener)
                {
                    resumeListener.OnResumeGame();
                }
            }
        }

        public void FinishGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IFinishGameListener finishListener)
                {
                    finishListener.OnFinishGame();
                }
            }
        }

        public void WinGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is IWinGameListener gameWinListener)
                {
                    gameWinListener.OnWinGame();
                }
            }
        }

        public void LoseGame()
        {
            foreach (var listener in _listeners)
            {
                if (listener is ILoseGameListener gameOverListener)
                {
                    gameOverListener.OnLoseGame();
                }
            }
        }
    }
}
