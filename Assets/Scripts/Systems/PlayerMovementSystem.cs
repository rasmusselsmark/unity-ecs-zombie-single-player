using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerData, transform) in SystemAPI.Query<RefRO<PlayerData>, RefRW<LocalTransform>>())
            {
                transform.ValueRW = transform.ValueRW.Translate(
                    transform.ValueRW.Forward() * playerData.ValueRO.Speed * SystemAPI.Time.DeltaTime);
                
                // freeze rotation of player in code
                transform.ValueRW.Rotation = quaternion.identity;
            }
        }
    }
}