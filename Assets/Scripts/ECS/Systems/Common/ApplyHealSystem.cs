using FreeTeam.BP.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems
{
    public class ApplyHealSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<HealData, HealthData>> filter = default;

        private readonly EcsPoolInject<HealData> healDataPool = default;
        private readonly EcsPoolInject<HealthData> healthDataPool = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var healData = ref healDataPool.Value.Get(entity);
                ref var healthData = ref healthDataPool.Value.Get(entity);

                healthData.Value = (int)Mathf.Min(healthData.Value + healData.Value, healthData.MaxValue);
            }
        }
        #endregion
    }
}
