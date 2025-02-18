using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial struct PlayerShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BulletSpawnerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spawner, playerTransform)
                     in SystemAPI.Query<RefRO<BulletSpawnerData>, RefRO<LocalTransform>>())
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("Fire1"))
                {
                    var bullet = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
                    state.EntityManager.SetName(bullet, "Bullet");

                    // place bullet in front of player
                    var transform = LocalTransform.FromPositionRotation(
                        playerTransform.ValueRO.Position,
                        new quaternion(playerTransform.ValueRO.Rotation.value));
                    transform = transform.Translate(transform.Forward() * 1f);
                    transform = transform.Translate(transform.Up() * 1f);

                    state.EntityManager.SetComponentData(bullet, transform);
                    state.EntityManager.SetComponentData(bullet, new ForwardMovementData
                    {
                        Speed = 20f,
                    });
                    state.EntityManager.SetComponentData(bullet, new EntityExpiresData
                    {
                        Seconds = 2f,
                    });
                }
            }
        }
    }
}
