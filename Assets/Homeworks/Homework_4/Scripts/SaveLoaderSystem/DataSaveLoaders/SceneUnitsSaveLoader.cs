using Zenject;

namespace SaveLoad
{
    public class SceneUnitsSaveLoader : SaveLoader<SceneUnitsService, SceneUnitsDataArray>
    {
        private DefaultSceneUnitsConfig _defaultSceneUnits;
        private SceneUnitConverter _sceneUnitConverter;


        [Inject]
        public void Construct(DefaultSceneUnitsConfig defaultSceneUnits, SceneUnitConverter sceneUnitConverter)
        {
            _defaultSceneUnits = defaultSceneUnits;
            _sceneUnitConverter = sceneUnitConverter;
        }

        protected override void SetupData(SceneUnitsDataArray data, SceneUnitsService service)
        {
            var gameObjects = _sceneUnitConverter.ConvertDataToGameObjectsArray(data);
            service.SetSceneObjects(gameObjects);

            DebugAlerts.AlertObjectsSirensOnLoad(data.DataArray);
        }

        protected override void SetupByDefault(SceneUnitsService service)
        {
            var data = _defaultSceneUnits.SceneUnitsData;
            var gameObjects = _sceneUnitConverter.ConvertDataToGameObjectsArray(data);
            service.SetSceneObjects(gameObjects);

            DebugAlerts.AlertOnDefaultLoadSceneObjects(data.DataArray);
        }

        protected override SceneUnitsDataArray ConvertToData(SceneUnitsService service)
        {
            var gameObjects = service.GetSceneObjects();
            var data = _sceneUnitConverter.ConvertGameObjectsToDataArray(gameObjects);

            DebugAlerts.AlertOnSaveSceneObjects(data.DataArray);
            
            return data;
        }
    }
}