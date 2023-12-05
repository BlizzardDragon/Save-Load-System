using Zenject;

namespace SaveLoad
{
    public class SceneResourcesSaveLoader : SaveLoader<SceneResourcesService, SceneResourcesDataArray>
    {
        private DefaultSceneResourcesConfig _defaultSceneResources;
        private SceneResourceConverter _sceneResourceConverter;


        [Inject]
        public void Construct(
            DefaultSceneResourcesConfig defaultSceneResources,
            SceneResourceConverter sceneResourceConverter)
        {
            _defaultSceneResources = defaultSceneResources;
            _sceneResourceConverter = sceneResourceConverter;
        }

        protected override void SetupData(SceneResourcesDataArray data, SceneResourcesService service)
        {
            var gameObjects = _sceneResourceConverter.ConvertDataToGameObjectsArray(data);
            service.SetSceneObjects(gameObjects);

            DebugAlerts.AlertObjectsSirensOnLoad(data.DataArray);
        }

        protected override void SetupByDefault(SceneResourcesService service)
        {
            var data = _defaultSceneResources.SceneResourcesData;
            var gameObjects = _sceneResourceConverter.ConvertDataToGameObjectsArray(data);
            service.SetSceneObjects(gameObjects);

            DebugAlerts.AlertOnDefaultLoadSceneObjects(data.DataArray);
        }

        protected override SceneResourcesDataArray ConvertToData(SceneResourcesService service)
        {
            var gameObjects = service.GetSceneObjects();
            var data = _sceneResourceConverter.ConvertGameObjectsToDataArray(gameObjects);
            
            DebugAlerts.AlertOnSaveSceneObjects(data.DataArray);

            return data;
        }
    }
}