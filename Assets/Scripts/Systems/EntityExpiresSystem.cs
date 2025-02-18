using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    /// <summary>
    /// Destroys entity after a certain amount of time
    /// </summary>
    public partial struct EntityExpiresSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new EntityDestructionJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                DeltaTime = SystemAPI.Time.DeltaTime,
            }.Schedule();
        }

        [BurstCompile]
        public partial struct EntityDestructionJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            public float DeltaTime;

            private void Execute(Entity entity, ref EntityExpiresData entityExpires)
            {
                entityExpires.Seconds -= DeltaTime;

                if (entityExpires.Seconds <= 0)
                {
                    ECB.DestroyEntity(entity);
                }
            }
        }
    }
}
