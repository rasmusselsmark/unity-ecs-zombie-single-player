using Unity.Entities;

namespace Components
{
    public struct BulletSpawnerData : IComponentData
    {
        public Entity BulletPrefab { get; set; }
    }
}
