using UnityEngine;

namespace SaveLoad
{
    public class SceneUnitsService : SceneObjectService<DefaultSceneUnitsConfig>
    {
        [ContextMenu(nameof(SaveObjectsByDefault))]
        public override void SaveObjectsByDefault()
        {
            base.SaveObjectsByDefault();
        }
    }
}