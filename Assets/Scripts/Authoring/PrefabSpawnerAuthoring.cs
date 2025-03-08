using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PrefabSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        [Tooltip("Name of spawned entities")]
        public string Name;
        [Tooltip("How often to spawn, i.e. seconds between spawns")]
        public float Interval = 1.0f;

        private class PrefabSpawnerAuthoringBaker : Baker<PrefabSpawnerAuthoring>
        {
            public override void Bake(PrefabSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic); // so we have LocalTransform in system
                var prefab = GetEntity(authoring.Prefab, TransformUsageFlags.None);

                AddComponent(entity, new PrefabSpawnerData
                {
                    Prefab = prefab,
                    Name = authoring.Name,
                    Interval = authoring.Interval,
                    TimeToNextSpawn = authoring.Interval,
                });
            }
        }
    }
}

