using FreeTeam.BP.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.EventSystem;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems
{
    public class HealthCheckerSystem :
        IEcsInitSystem,
        IEcsDestroySystem,
        EventSystem<HealthData>.IComponentChangedListener
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsPoolInject<EntitiesDependenciesData> entitiesDependenciesDataPool = default;
        private readonly EcsPoolInject<InactiveEntityFlag> InactiveEntityFlagPool = default;
        private readonly EcsPoolInject<ExplosionData> explosionDataPool = default;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems) =>
            world.Value.AddChangedListener(this);

        public void Destroy(IEcsSystems systems) =>
            world.Value.RemoveChangedListener(this);

        public void OnComponentChanged(EcsWorld world, int entity, HealthData? data, EntityChangeTypes changeType)
        {
            if (changeType != EntityChangeTypes.Changed)
                return;

            if (data?.Value > 0)
                return;

            if (InactiveEntityFlagPool.Value.Has(entity))
                return;

            Debug.Log($"<b>[{GetType().Name}]</b> | {{E{entity:D8}:{data?.GetType().Name}:{changeType}}} | Value={data?.Value}");

            var explosionEntity = world.NewEntity();
            ref var explosionData = ref explosionDataPool.Value.Add(explosionEntity);
            explosionData.entity = world.PackEntity(entity);
            explosionData.Timer = UnityEngine.Random.Range(0f, 0.25f);

            if (!entitiesDependenciesDataPool.Value.Has(entity))
                return;

            ref var entitiesDependenciesData = ref entitiesDependenciesDataPool.Value.Get(entity);
            foreach (var packedEntity in entitiesDependenciesData.Entities)
            {
                if (!packedEntity.Unpack(world, out var ed))
                    continue;

                InactiveEntityFlagPool.Value.Replace(ed);
            }
        }
        #endregion
    }
}
