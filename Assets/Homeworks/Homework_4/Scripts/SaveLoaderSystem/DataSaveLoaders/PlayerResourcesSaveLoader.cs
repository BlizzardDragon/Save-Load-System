using System.Collections.Generic;

namespace SaveLoad
{
    public class PlayerResourcesSaveLoader : SaveLoader<PlayerResourcesService, PlayerResourceData>
    {
        private Dictionary<ResourceType, int> _startResources = new()
        {
            {ResourceType.FOOD, 20},
            {ResourceType.MONEY, 50},
            {ResourceType.STONE, 5},
            {ResourceType.WOOD, 10}
        };

        private const string RESOURCE = "Resource";


        protected override void SetupData(PlayerResourceData data, PlayerResourcesService _service)
        {
            foreach (var resource in data.Resources)
            {
                _service.SetResource(resource.Key, resource.Value);
                DebugAlerts.AlertOnLoad(RESOURCE + " " + resource.Key.ToString(), resource.Value);
            }
        }

        protected override void SetupByDefault(PlayerResourcesService service)
        {
            foreach (var resource in _startResources)
            {
                service.SetResource(resource.Key, resource.Value);
                DebugAlerts.AlertOnDefaultLoad(RESOURCE + " " + resource.Key.ToString(), resource.Value);
            }
        }

        protected override PlayerResourceData ConvertToData(PlayerResourcesService service)
        {
            var resourceData = new PlayerResourceData
            {
                Resources = service.GetResources()
            };

            foreach (var resource in resourceData.Resources)
            {
                DebugAlerts.AlertOnSave(RESOURCE + " " + resource.Key.ToString(), resource.Value);
            }

            return resourceData;
        }
    }
}
