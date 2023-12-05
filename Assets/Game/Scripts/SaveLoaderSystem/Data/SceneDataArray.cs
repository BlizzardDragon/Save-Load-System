using System;

namespace SaveLoad
{
    [Serializable]
    public class SceneDataArray <TDataArray> where TDataArray : GameObjectData
    {
        public TDataArray[] DataArray;
    }
}