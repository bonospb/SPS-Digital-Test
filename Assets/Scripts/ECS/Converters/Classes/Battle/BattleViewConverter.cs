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
    public class BattleViewConverter : MonoBehaviour, IViewConverter, IViewBinder, IViewWithDependencies
    {
        #region SerializeFields
        [SerializeField] private BattleView view = null;
        #endregion

        #region Private
        private Configurations configurations = null;
        #endregion

        #region Public methods
        [Inject]
        public void Construct(Configurations config) =>
            configurations = config;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var config = configurations.Avatars[view.ConfigId];

            var groupDataPool = world.GetPool<GroupData>();
            ref var groupData = ref groupDataPool.Add(entity);
            groupData.Group = view.Group;

            var battleUnitDataPool = world.GetPool<BattleUnitData>();
            battleUnitDataPool.Add(entity);

            var agrDataPool = world.GetPool<AgrData>();
            ref var agrData = ref agrDataPool.Add(entity);
            agrData.Range = new Range()
            {
                Min = 0,
                Max = 25f
            };

            var healthDataPool = world.GetPool<HealthData>();
            ref var healthData = ref healthDataPool.Add(entity);
            healthData.Value = config.Health;
            healthData.MaxValue = (uint)config.Health;

            var transformRefPool = world.GetPool<TransformRef>();
            ref var transformRef = ref transformRefPool.Add(entity);
            transformRef.Transform = view.GetTransform();

            var configRefPool = world.GetPool<ConfigRef>();
            ref var configRef = ref configRefPool.Add(entity);
            configRef.Config = config;
        }

        public void Binding(EcsWorld world, Dictionary<IEntityView, int> entities)
        {
            var entity = entities[view];

            var armPlaceEntities = entities
                .Where(x => x.Key is IArmPlaceEntityView)
                .Select(x => world.PackEntity(x.Value))
                .ToArray();

            var battleUnitDataPool = world.GetPool<BattleUnitData>();
            ref var battleUnitData = ref battleUnitDataPool.Get(entity);
            battleUnitData.ArmPlaceEntities = armPlaceEntities;
        }

        public void SetEntityDependencies(EcsWorld world, int entity, IEnumerable<EcsPackedEntity> entities) =>
            world.GetPool<EntitiesDependenciesData>().Add(entity).Entities = entities;
        #endregion
    }
}
