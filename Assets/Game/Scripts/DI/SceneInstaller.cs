using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private DefaultSceneUnitsConfig _defaultSceneUnitsConfig;
        [SerializeField] private DefaultSceneResourcesConfig _defaultSceneResourcesConfig;


        public override void InstallBindings()
        {
            BindSaveLoadSystems();
            BindServices();
            BindConverters();
            BindConfigs();
        }

        private void BindSaveLoadSystems()
        {
            Container.Bind<SaveLoadManager>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<ISaveLoader>().To<PlayerResourcesSaveLoader>().AsCached();
            Container.Bind<ISaveLoader>().To<SceneResourcesSaveLoader>().AsCached();
            Container.Bind<ISaveLoader>().To<SceneUnitsSaveLoader>().AsCached();
        }

        private void BindServices()
        {
            Container.Bind<PlayerResourcesService>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SceneResourcesService>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SceneUnitsService>().FromComponentInHierarchy().AsSingle();
        }

        private void BindConverters()
        {
            Container.Bind<SceneUnitConverter>().AsSingle();
            Container.Bind<SceneResourceConverter>().AsSingle();
        }

        private void BindConfigs()
        {
            Container.BindInstance(_defaultSceneUnitsConfig).AsSingle();
            Container.BindInstance(_defaultSceneResourcesConfig).AsSingle();
        }

        public override void Start()
        {
            base.Start();

            Container.Inject(_defaultSceneUnitsConfig);
            Container.Inject(_defaultSceneResourcesConfig);
        }
    }
}
