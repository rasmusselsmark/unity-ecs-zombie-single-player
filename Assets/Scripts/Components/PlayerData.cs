using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct PlayerData : IComponentData
    {
        public float Speed { get; set; }
        public float JumpHeight { get; set; }
        public float TurnSpeed { get; set; }

        /// <summary>
        /// Which euler angle is player pointing on y-axis
        /// </summary>
        internal float Rotation { get; set; }

        /// <summary>
        /// Vertical velocity of the player, used when jumping
        /// </summary>
        internal float3 JumpVelocity { get; set; }
    }
}
