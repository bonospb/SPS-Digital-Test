using FreeTeam.BP.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BP.ECS.Systems
{
    public class ApplyDamageSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<DamageData, HealthData>> filter = default;

        private readonly EcsPoolInject<DamageData> damageDataPool = default;
        private readonly EcsPoolInject<HealthData> healthDataPool = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var damageData = ref damageDataPool.Value.Get(entity);
                ref var healthData = ref healthDataPool.Value.Get(entity);

                healthData.Value -= (int)damageData.Value;
            }
        }
        #endregion
    }
}
