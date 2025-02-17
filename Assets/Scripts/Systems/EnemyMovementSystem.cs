using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial struct EnemyMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<EnemyMovementData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // this only works as long as there is only one player
            foreach (var (playerTransform, _) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerTag>>())
            {
                foreach (var (enemyTransform, movement, mass, velocity)
                         in SystemAPI.Query<RefRW<LocalTransform>, RefRO<EnemyMovementData>, RefRO<PhysicsMass>,
                             RefRW<PhysicsVelocity>>())
                {
                    var playerPosition = playerTransform.ValueRO.Position;

                    // find rotation towards player
                    var direction =
                        new float3(playerPosition.x, playerPosition.y, playerPosition.z)
                        - enemyTransform.ValueRW.Position;
                    var targetRotation = quaternion.LookRotationSafe(direction, math.up());

                    enemyTransform.ValueRW.Rotation = math.nlerp(
                        enemyTransform.ValueRW.Rotation.value,
                        targetRotation,
                        math.PI * SystemAPI.Time.DeltaTime);

                    // Move forward
                    enemyTransform.ValueRW.Position +=
                        enemyTransform.ValueRW.Forward() * movement.ValueRO.Speed * SystemAPI.Time.DeltaTime;

                    // TODO: Use physics for moving
                    // velocity.ValueRW = PhysicsVelocity.CalculateVelocityToTarget(
                    //     bodyMass: mass.ValueRO,
                    //     bodyPosition: enemyTransform.ValueRO.Position,
                    //     bodyOrientation: enemyTransform.ValueRO.Rotation,
                    //     targetTransform: new RigidTransform(
                    //         targetRotation,
                    //         // quaternion.LookRotation(playerTransform.ValueRO.Position, enemyTransform.ValueRW.Up()),
                    //         enemyTransform.ValueRO.Position + movement.ValueRO.Speed),
                    //     stepFrequency: 1f / SystemAPI.Time.DeltaTime);
                }

                break; // TODO: when multiple players, find closest playerene
            }
        }
    }
}
