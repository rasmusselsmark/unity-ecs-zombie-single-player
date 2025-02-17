using Unity.Collections;
using Unity.Entities;

namespace Components
{
    public struct PrefabSpawnerData : IComponentData
    {
        public Entity Prefab { get; set; }
        public float Interval { get; set; }

        internal float TimeToNextSpawn;
        // public FixedString32Bytes Name { get; set; }
    }
}
