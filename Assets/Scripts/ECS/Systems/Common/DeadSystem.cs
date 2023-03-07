using FreeTeam.BP.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems
{
    public class DeadSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<DeadData>, Exc<DestroyEntityEvent>> filter = default;

        private readonly EcsPoolInject<DeadData> deadDataPool = default;
        private readonly EcsPoolInject<DestroyEntityEvent> destroyEntityEventPool = default;
        private readonly EcsPoolInject<TransformRef> transformRefPool = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var deadData = ref deadDataPool.Value.Get(entity);
                if (deadData.Timer <= 0)
                {
                    destroyEntityEventPool.Value.Add(entity).WithDependencies = true;

                    if (transformRefPool.Value.Has(entity))
                    {
                        ref var transformRef = ref transformRefPool.Value.Get(entity);
                        transformRef.Transform.gameObject.SetActive(false);
                    }
                }

                deadData.Timer -= Time.fixedDeltaTime;
            }
        }
        #endregion
    }
}
