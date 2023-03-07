using FreeTeam.BP.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.EventSystem;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems
{
    public class DestroyEntitySystem :
        IEcsInitSystem,
        IEcsDestroySystem,
        IEcsRunSystem,
        EventSystem<DestroyEntityEvent>.IComponentChangedListener
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsFilterInject<Inc<DestroyEntityEvent>> filter = default;

        private readonly EcsPoolInject<DestroyEntityEvent> destroyEntityEventPool = default;
        private readonly EcsPoolInject<EntitiesDependenciesData> entitiesDependenciesDataPool = default;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems) =>
            world.Value.AddChangedListener(this);

        public void Destroy(IEcsSystems systems) =>
            world.Value.RemoveChangedListener(this);

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var destroyEntityEvent = ref destroyEntityEventPool.Value.Get(entity);

                if (destroyEntityEvent.WithDependencies && entitiesDependenciesDataPool.Value.Has(entity))
                {
                    ref var entitiesDependenciesData = ref entitiesDependenciesDataPool.Value.Get(entity);
                    foreach (var entityDependency in entitiesDependenciesData.Entities)
                    {
                        if (!entityDependency.Unpack(world.Value, out var dEntity))
                            continue;

                        world.Value.DelEntity(dEntity);
                    }
                }
                else
                {
                    world.Value.DelEntity(entity);
                }
            }
        }

        public void OnComponentChanged(EcsWorld world, int entity, DestroyEntityEvent? data, EntityChangeTypes changeType) =>
            Debug.Log($"<b>[{GetType().Name}]</b> | {{E:{entity:D8}->C:{data?.GetType().Name}->{changeType}}} | WithDependencies={data?.WithDependencies}");
        #endregion
    }
}
