using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(TransformSystemGroup))] // ensure entity is rendered at correct position
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
                spawner.ValueRW.TimeToNextSpawn -= SystemAPI.Time.DeltaTime;
                if (spawner.ValueRW.TimeToNextSpawn > 0)
                    continue;

                spawner.ValueRW.TimeToNextSpawn = spawner.ValueRO.Interval;

                var go = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
                state.EntityManager.SetName(go, spawner.ValueRO.Name);

                // place spawned object at position of spawner
                var transform = LocalTransform.FromPositionRotation(
                    spawnerTransform.ValueRO.Position,
                    new quaternion(spawnerTransform.ValueRO.Rotation.value));
                state.EntityManager.SetComponentData(go, transform);
            }
        }
    }
}
