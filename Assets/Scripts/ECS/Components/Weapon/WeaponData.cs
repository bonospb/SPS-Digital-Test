using Leopotam.EcsLite;

namespace FreeTeam.BP.ECS.Components
{
    public struct WeaponData
    {
        public float CooldownTimer;

        public EcsPackedEntity[] EffectsEntities;
    }
}
