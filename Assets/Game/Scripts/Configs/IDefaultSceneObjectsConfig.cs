using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    public interface IDefaultSceneObjectsConfig
    {
        void SaveObjects(List<GameObject> gameObjects);
    }
}