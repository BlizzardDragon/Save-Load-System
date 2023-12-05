using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    [CreateAssetMenu(
        fileName = "DefaultSceneResourcesConfig", 
        menuName = "ScriptableObjects/DefaultSceneResourcesConfig", 
        order = 0)]
    public class DefaultSceneResourcesConfig : ScriptableObject, IDefaultSceneObjectsConfig
    {
        [SerializeField] private SceneResourcesDataArray _sceneResourcesData;
        private SceneResourceConverter _sceneResourceConverter;

        public SceneResourcesDataArray SceneResourcesData => _sceneResourcesData;      


        [Inject]
        public void Construct(SceneResourceConverter sceneResourceConverter)
        {
            _sceneResourceConverter = sceneResourceConverter;
        }

        public void SaveObjects(List<GameObject> gameObjects)
        {
            _sceneResourcesData = _sceneResourceConverter.ConvertGameObjectsToDataArray(gameObjects);
            EditorUtility.SetDirty(this);
        }
    }
}
