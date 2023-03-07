using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FreeTeam.BP.Behaviours.Converters
{
    public class CollisionViewConverter : MonoBehaviour, IViewConverter, IViewBinder
    {
        #region SerializeFields
        [SerializeField] private CollisionView view = null;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var colliderRefPool = world.GetPool<ColliderRef>();
            ref var colliderRef = ref colliderRefPool.Add(entity);
            colliderRef.Collider = view.GetCollider();
        }

        public void Binding(EcsWorld world, Dictionary<IEntityView, int> entities)
        {
            var entity = entities[view];

            var damageCollisionDataPool = world.GetPool<DamageCollisionData>();
            ref var damageCollisionData = ref damageCollisionDataPool.Add(entity);
            damageCollisionData.BattleEntity = world.PackEntity(entities.First(x => x.Key is IBattleEntityView).Value);
        }
        #endregion
    }
}
