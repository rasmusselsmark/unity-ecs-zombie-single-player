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
                     in SystemAPI.Query<RefRW<BulletSpawnerData>, RefRW<LocalTransform>>())
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    var bullet = state.EntityManager.Instantiate(spawner.ValueRO.BulletPrefab);
                    state.EntityManager.SetName(bullet, "Bullet");
                    // state.EntityManager.SetComponentData(
                    //     bullet,
                    //     LocalTransform.FromPosition(position.ValueRO.Position));

                    var position = playerTransform.ValueRO.Position;

                    // position bullet in front of player
                    var transform = LocalTransform.FromPositionRotation(
                        new float3(position.x, position.y, position.z),
                        new quaternion(playerTransform.ValueRO.Rotation.value));
                    transform = transform.Translate(transform.Forward() * 2f);

                    state.EntityManager.SetComponentData(bullet, transform);
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}
