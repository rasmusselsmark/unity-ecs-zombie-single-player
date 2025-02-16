using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

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
            var deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (playerData, transform, mass, velocity) in SystemAPI
                .Query<RefRW<PlayerData>, RefRW<LocalTransform>, RefRO<PhysicsMass>, RefRW<PhysicsVelocity>>())
            {
                // rotation
                playerData.ValueRW.Rotation +=
                    Input.GetAxis("Horizontal") * playerData.ValueRO.TurnSpeed * deltaTime;
                var rotation = quaternion.Euler(0f, playerData.ValueRW.Rotation, 0f);

                // jump
                // TODO: implement collider/grounding and lerp jump velocity
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerData.ValueRW.JumpVelocity = transform.ValueRW.Up() * playerData.ValueRO.JumpHeight;
                }
                else if (playerData.ValueRW.JumpVelocity.y > 0f)
                {
                    playerData.ValueRW.JumpVelocity = -math.abs(playerData.ValueRW.JumpVelocity * deltaTime);
                }

                // movement
                var forward = Input.GetAxis("Vertical") * transform.ValueRW.Forward()
                                                        * playerData.ValueRO.Speed * deltaTime;
                velocity.ValueRW = PhysicsVelocity.CalculateVelocityToTarget(
                    bodyMass: mass.ValueRO,
                    bodyPosition: transform.ValueRO.Position,
                    bodyOrientation: transform.ValueRO.Rotation,
                    targetTransform: new RigidTransform(
                        rotation,
                        transform.ValueRO.Position
                        + forward
                        + playerData.ValueRW.JumpVelocity),
                    stepFrequency: 1f / deltaTime);
            }
        }
    }
}
