using System.Linq;
using FrameworkUnity.OOP.Interfaces.Listeners;
using UnityEngine;
using Zenject;

namespace FrameworkUnity.OOP.Zenject.Installers
{
    public enum FindTypes
    {
        FindObjectsOfType,
        GetComponentsInChildren,
    }

    [RequireComponent(typeof(GameManagerZenject))]
    public abstract class BaseGameSystemsInstaller : MonoInstaller<BaseGameSystemsInstaller>
    {
        [Header("AutoRun")]
        [SerializeField] private bool _initGame = true;
        [SerializeField] private bool _prepareGame = true;
        [SerializeField] private bool _startGame = true;

        [Header("Listeners")]
        [SerializeField] private bool _gameListenersIsMono = true;
        [SerializeField] private FindTypes _findType;
        
        private GameManagerZenject _gameManager;


        public override void InstallBindings()
        {
            InstallGameListeners();
            InstallGameManager();
            InstallGameSystems();
        }

        private void Awake()
        {
            if (_initGame)
            {
                _gameManager.InitGame();
            }
        }

        public override void Start()
        {
            base.Start();

            if (_prepareGame)
            {
                _gameManager.PrepareGame();
            }

            if (_startGame)
            {
                _gameManager.StartGame();
            }
        }

        private void InstallGameListeners()
        {
            if (!_gameListenersIsMono) return;

            if (_findType == FindTypes.FindObjectsOfType)
            {
                foreach (var gameListener in FindObjectsOfType<MonoBehaviour>().OfType<IGameListener>())
                {
                    Container.BindInterfacesTo(gameListener.GetType()).FromInstance(gameListener).AsCached();
                }
            }
            else if (_findType == FindTypes.GetComponentsInChildren)
            {
                foreach (var gameListener in GetComponentsInChildren<IGameListener>(true))
                {
                    Container.BindInterfacesTo(gameListener.GetType()).FromInstance(gameListener).AsCached();
                }
            }
        }

        private void InstallGameManager()
        {
            _gameManager = GetComponent<GameManagerZenject>();
            Container.Bind<GameManagerContext>().AsCached();
            Container.Bind<GameManagerZenject>().FromInstance(_gameManager).AsCached();
        }

        protected abstract void InstallGameSystems();
    }
}
