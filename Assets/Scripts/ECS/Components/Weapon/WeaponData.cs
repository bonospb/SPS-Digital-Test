using Leopotam.EcsLite;

namespace FreeTeam.BP.ECS.Components
{
    public struct WeaponData
    {
        public float RotationSpeed;

        public float Cooldown;

        public float Timer;

        public EcsPackedEntity[] EffectsEntities;
    }
}
