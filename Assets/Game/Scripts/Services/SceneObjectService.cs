using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    public abstract class SceneObjectService<TConfig> : MonoBehaviour 
        where TConfig : IDefaultSceneObjectsConfig
    {
        [Header("Object section")]
        [SerializeField] protected Transform _spawnParent;
        [SerializeField] protected List<GameObject> _sceneObjects;

        [Space]
        [Header("Defoult config section")]
        [SerializeField] private TConfig _defaultConfig;


        public void SetSceneObjects(GameObject[] objects)
        {
            foreach (var obj in objects)
            {
                obj.transform.parent = _spawnParent;
            }

            _sceneObjects = new(objects);
        }

        public List<GameObject> GetSceneObjects() => _sceneObjects;
        public void AddObject(GameObject obj) => _sceneObjects.Add(obj);
        public void RemoveObject(GameObject obj) => _sceneObjects.Remove(obj);

        public virtual void SaveObjectsByDefault() => _defaultConfig.SaveObjects(_sceneObjects);
    }
}