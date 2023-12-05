using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaveLoad
{
    public sealed class PlayerResourcesService : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private Dictionary<ResourceType, int> _resources = new();


        public void SetResource(ResourceType resourceType, int resource)
        {
            _resources[resourceType] = resource;
        }
        
        public int GetResource(ResourceType resourceType)
        {
            return _resources[resourceType];
        }
        
        public Dictionary<ResourceType, int> GetResources()
        {
            return _resources;
        }
    }
}