using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class EntityExpiresAuthoring : MonoBehaviour
    {
        public float Seconds = 1.0f;
        private class EntityExpiresAuthoringBaker : Baker<EntityExpiresAuthoring>
        {
            public override void Bake(EntityExpiresAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EntityExpiresData
                {
                    Seconds = authoring.Seconds,
                });
            }
        }
    }
}

