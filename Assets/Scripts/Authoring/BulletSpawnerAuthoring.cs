using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class BulletSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefab;

        private class BulletSpawnerBaker : Baker<BulletSpawnerAuthoring>
        {
            public override void Bake(BulletSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var prefab = GetEntity(authoring.Prefab, TransformUsageFlags.None);

                AddComponent(entity, new BulletSpawnerData
                {
                    Prefab = prefab,
                });
            }
        }
    }
}

