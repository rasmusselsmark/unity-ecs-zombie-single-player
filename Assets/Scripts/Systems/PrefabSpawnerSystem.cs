using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial struct PrefabSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PrefabSpawnerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spawner, spawnerTransform)
                     in SystemAPI.Query<RefRW<PrefabSpawnerData>, RefRO<LocalTransform>>())
            {
                spawner.ValueRW.Interval -= SystemAPI.Time.DeltaTime;
                if (spawner.ValueRW.Interval > 0)
                    continue;

                spawner.ValueRW.Interval = 1;

                var go = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
                // state.EntityManager.SetName(go, spawner.ValueRO.Name);

                // place spawned object at position of spawner
                var transform = LocalTransform.FromPositionRotation(
                    spawnerTransform.ValueRO.Position,
                    new quaternion(spawnerTransform.ValueRO.Rotation.value));
                state.EntityManager.SetComponentData(go, transform);
            }
        }
    }
}
