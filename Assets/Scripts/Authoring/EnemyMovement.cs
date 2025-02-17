using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class EnemyMovement : MonoBehaviour
    {
        public float Speed;

        private class EnemyMovementBaker : Baker<EnemyMovement>
        {
            public override void Bake(EnemyMovement authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnemyMovementData
                {
                    Speed = authoring.Speed,
                });
            }
        }
    }
}
