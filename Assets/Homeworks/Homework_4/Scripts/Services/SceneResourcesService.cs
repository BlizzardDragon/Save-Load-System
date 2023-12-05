using UnityEngine;

namespace SaveLoad
{
    public class SceneResourcesService : SceneObjectService<DefaultSceneResourcesConfig>
    {
        [ContextMenu(nameof(SaveObjectsByDefault))]
        public override void SaveObjectsByDefault()
        {
            base.SaveObjectsByDefault();
        }
    }
}