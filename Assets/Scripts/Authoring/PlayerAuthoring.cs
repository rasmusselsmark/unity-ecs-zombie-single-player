using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float Speed;
        public float JumpHeight;
        public float TurnSpeed;

        private class PlayerAuthoringBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerData
                {
                    Speed = authoring.Speed,
                    JumpHeight = authoring.JumpHeight,
                    TurnSpeed = authoring.TurnSpeed,
                });
            }
        }
    }
}
