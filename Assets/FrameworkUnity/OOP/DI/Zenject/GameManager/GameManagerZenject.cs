using System;
using UnityEngine;
using FrameworkUnity.OOP.Zenject.Installers;
using Zenject;

namespace FrameworkUnity.OOP.Zenject
{
    public enum GameState
    {
        Off = 0,
        Preparing = 1,
        Playing = 2,
        Pause = 3,
        Finish = 4,
        Win = 5,
        Lose = 6
    }

    [RequireComponent(typeof(BaseGameSystemsInstaller))]
    public sealed class GameManagerZenject : MonoBehaviour
    {
        public GameState State { get; private set; }
        private float _fixedDeltaTime;

        public event Action OnInitGame;
        public event Action OnDeInitGame;
        public event Action OnPrepareForGame;
        public event Action OnStartGame;
        public event Action OnPauseGame;
        public event Action OnResumeGame;
        public event Action OnFinishGame;
        public event Action OnWinGame;
        public event Action OnLoseGame;

        [Inject]
        private GameManagerContext _context;


        private void Update()
        {
            if (State != GameState.Playing) return;

            _context.OnUpdate();
        }

        private void FixedUpdate()
        {
            if (State != GameState.Playing) return;

            _context.OnFixedUpdate(_fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (State != GameState.Playing) return;

            _context.OnLateUpdate();
        }

        internal void InitGame()
        {
            _fixedDeltaTime = Time.fixedDeltaTime;
            _context.InitGame();
            OnInitGame?.Invoke();
        }

        internal void DeInitGame()
        {
            _context.DeInitGame();
            OnDeInitGame?.Invoke();
        }

        public void PrepareGame()
        {
            _context.PrepareGame();
            State = GameState.Preparing;
            OnPrepareForGame?.Invoke();
        }

        public void StartGame()
        {
            _context.StartGame();
            State = GameState.Playing;
            OnStartGame?.Invoke();
        }

        public void PauseGame()
        {
            _context.PauseGame();
            State = GameState.Pause;
            OnPauseGame?.Invoke();
        }

        public void ResumeGame()
        {
            _context.ResumeGame();
            State = GameState.Playing;
            OnResumeGame?.Invoke();
        }

        public void WinGame()
        {
            _context.WinGame();
            State = GameState.Win;
            OnWinGame?.Invoke();
            DeInitGame();
        }

        public void LoseGame()
        {
            _context.LoseGame();
            State = GameState.Lose;
            OnLoseGame?.Invoke();
            DeInitGame();
        }

        public void FinishGame()
        {
            _context.FinishGame();
            State = GameState.Finish;
            OnFinishGame?.Invoke();
            DeInitGame();
        }
    }
}