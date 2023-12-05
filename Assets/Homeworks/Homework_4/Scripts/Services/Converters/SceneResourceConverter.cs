using System.Collections.Generic;

namespace SaveLoad
{
    public class SceneResourceConverter :
        SceneObjectConverter<SceneResourceData, SceneResourcesDataArray, ResourceObject, ResourceType>
    {
        public SceneResourceConverter()
        {
            _prefabLinks = new()
            {
                {ResourceType.STONE, "SceneResources/Stone"},
                {ResourceType.WOOD, "SceneResources/Tree"},
            };
        }


        protected override bool CheckForType(SceneResourceData data)
        {
            return _prefabLinks.ContainsKey(data.Type);
        }

        protected override string GetLinkToPrefab(SceneResourceData data, Dictionary<ResourceType, string> _prefabLinks)
        {
            return _prefabLinks[data.Type];
        }

        protected override void SetupObject(SceneResourceData data, ResourceObject gameObj)
        {
            gameObj.RemainingCount = data.RemainingCount;
            gameObj.ResourceType = data.Type;

            if (data.RemainingCount < 1)
            {
                gameObj.gameObject.SetActive(false);
            }
        }

        protected override void SetupData(SceneResourceData data, ResourceObject gameObj)
        {
            data.Type = gameObj.ResourceType;
            data.RemainingCount = gameObj.RemainingCount;
        }
    }
}