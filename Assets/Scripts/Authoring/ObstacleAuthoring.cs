using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class ObstacleAuthoring : MonoBehaviour
    {
        private class ObstacleAuthoringBaker : Baker<ObstacleAuthoring>
        {
            public override void Bake(ObstacleAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<ObstacleTag>(entity);
            }
        }
    }
}

