using Leopotam.EcsLite;
using UnityEngine;

namespace FreeTeam.BP.ECS.Components
{
    public struct ShotData
    {
        public EcsPackedEntity WeaponEntity;

        public int OriginGroup;

        public Vector3 Point;
        public Vector3 Direction;
    }
}
