using UnityEngine;

namespace SaveLoad
{
    public sealed class ResourceObject : MonoBehaviour
    {
        [SerializeField]
        private ResourceType resourceType;

        [SerializeField]
        private int remainingCount;

        public ResourceType ResourceType { get => resourceType; set => resourceType = value; }
        public int RemainingCount { get => remainingCount; set => remainingCount = value; }
    }
}