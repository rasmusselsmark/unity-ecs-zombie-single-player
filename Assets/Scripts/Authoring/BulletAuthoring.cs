using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class BulletAuthoring : MonoBehaviour
    {
        private class BulletAuthoringBaker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BulletTag>(entity);
                AddComponent(entity, new ForwardMovementData()
                {
                    Speed = 2f,
                });
            }
        }
    }
}
