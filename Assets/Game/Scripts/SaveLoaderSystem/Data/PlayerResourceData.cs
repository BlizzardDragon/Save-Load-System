using System;
using System.Collections.Generic;

namespace SaveLoad
{
    [Serializable]
    public struct PlayerResourceData
    {
        public Dictionary<ResourceType, int> Resources;
    }
}