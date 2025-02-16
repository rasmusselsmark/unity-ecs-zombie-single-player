using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public partial struct ForwardMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, movement)
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<ForwardMovementData>>())
            {
                transform.ValueRW = transform.ValueRW.Translate(
                    transform.ValueRW.Forward() * movement.ValueRO.Speed * SystemAPI.Time.DeltaTime);
            }
        }
    }
}
