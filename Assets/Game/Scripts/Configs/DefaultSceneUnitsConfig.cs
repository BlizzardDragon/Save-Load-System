using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    [CreateAssetMenu(
        fileName = "DefaultSceneUnitsConfig",
        menuName = "ScriptableObjects/DefaultSceneUnitsConfig",
        order = 0)]
    public class DefaultSceneUnitsConfig : ScriptableObject, IDefaultSceneObjectsConfig
    {
        [SerializeField] private SceneUnitsDataArray _sceneUnitsData;
        private SceneUnitConverter _sceneUnitConverter;

        public SceneUnitsDataArray SceneUnitsData => _sceneUnitsData;


        [Inject]
        public void Construct(SceneUnitConverter sceneUnitConverter)
        {
            _sceneUnitConverter = sceneUnitConverter;
        }

        public void SaveObjects(List<GameObject> gameObjects)
        {
            _sceneUnitsData = _sceneUnitConverter.ConvertGameObjectsToDataArray(gameObjects);
            EditorUtility.SetDirty(this);
        }
    }
}