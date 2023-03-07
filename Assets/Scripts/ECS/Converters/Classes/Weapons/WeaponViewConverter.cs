using FreeTeam.BP.Configuration;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.Behaviours.Converters
{
    public class WeaponViewConverter : MonoBehaviour, IViewConverter, IViewBinder
    {
        #region SerializeFileds
        [SerializeField] private WeaponView view = null;
        #endregion

        #region Private
        private Configurations configurations = null;
        #endregion

        #region Public methods
        [Inject]
        public void Construct(EcsWorld ecsWorld, Configurations configs) =>
            configurations = configs;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var config = configurations.Arms[view.ConfigId];

            var armConfigRefPool = world.GetPool<ConfigRef>();
            ref var armConfigRef = ref armConfigRefPool.Add(entity);
            armConfigRef.Config = config;

            var weaponDataPool = world.GetPool<WeaponData>();
            ref var weaponData = ref weaponDataPool.Add(entity);
            weaponData.CooldownTimer = 0f;

            var transformRefPool = world.GetPool<TransformRef>();
            ref var transformRef = ref transformRefPool.Add(entity);
            transformRef.Transform = view.GetTransform();

            var pointTransformRefPool = world.GetPool<PointTransformRef>();
            ref var pointTransformRef = ref pointTransformRefPool.Add(entity);
            pointTransformRef.PointTransform = view.GetPointTransform();
        }

        public void Binding(EcsWorld world, Dictionary<IEntityView, int> entities)
        {
            var entity = entities[view];

            var allEffectsEntities = entities.Where(x => x.Key is IEffectsEntityView).Select(x => world.PackEntity(x.Value)).ToArray();

            var weaponDataPool = world.GetPool<WeaponData>();
            ref var weaponData = ref weaponDataPool.Get(entity);
            weaponData.EffectsEntities = allEffectsEntities;
        }
        #endregion
    }
}
