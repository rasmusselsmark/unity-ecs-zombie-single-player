using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PlayerTagAuthoring : MonoBehaviour
    {
        private class PlayerTagAuthoringBaker : Baker<PlayerTagAuthoring>
        {
            public override void Bake(PlayerTagAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(entity);
            }
        }
    }
}

