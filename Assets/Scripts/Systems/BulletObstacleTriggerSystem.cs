using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] // = FixedUpdate()
    [UpdateAfter(typeof(PhysicsSystemGroup))] // wait for other physics events to complete
    public partial struct BulletObstacleTriggerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            state.Dependency = new BulletObstacleTriggerJob
            {
                ObstacleGroup = SystemAPI.GetComponentLookup<ObstacleTag>(),
                BulletGroup = SystemAPI.GetComponentLookup<BulletTag>(),
                ECB = ecb,
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            state.Dependency.Complete();
        }

        private struct BulletObstacleTriggerJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentLookup<ObstacleTag> ObstacleGroup;
            [ReadOnly] public ComponentLookup<BulletTag> BulletGroup;
            public EntityCommandBuffer ECB;

            public void Execute(TriggerEvent triggerEvent)
            {
                var obstacle = Utils.TriggerEventHasComponent(triggerEvent, ObstacleGroup);
                var bullet = Utils.TriggerEventHasComponent(triggerEvent, BulletGroup);

                if (obstacle != Entity.Null && bullet != Entity.Null)
                {
                    ECB.DestroyEntity(bullet);
                }
            }
        }
    }
}
