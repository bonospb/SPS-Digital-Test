using Leopotam.EcsLite;
using UnityEngine;

namespace FreeTeam.BP.ECS.Components
{
    public struct HitData
    {
        public EcsPackedEntity WeaponEntity;
        public EcsPackedEntity TargetEntity;

        public Vector3 StartPoint;
        public Vector3 Normal;
    }
}
