using Unity.Entities;

namespace Components
{
    public struct PlayerData : IComponentData
    {
        public float Speed { get; set; }
        public float JumpForce { get; set; }
    }
}