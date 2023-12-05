using System;
using UnityEngine;

namespace SaveLoad
{
    [Serializable]
    public class SceneResourcesDataArray : SceneDataArray<SceneResourceData>
    {
    }

    [Serializable]
    public class SceneResourceData : GameObjectData, IObjectType<ResourceType>
    {
        public int RemainingCount;
        [field: SerializeField] public ResourceType Type { get; set; }
    }
}