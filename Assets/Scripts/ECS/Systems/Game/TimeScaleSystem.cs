using FreeTeam.BP.Common;
using FreeTeam.BP.Configuration;
using FreeTeam.BP.Data.Constants;
using FreeTeam.BP.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.EventSystem;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems
{
    public class TimeScaleSystem :
        IEcsInitSystem,
        IEcsDestroySystem,
        EventSystem<TimeScaleData>.IComponentChangedListener
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsCustomInject<Configurations> config = default;
        #endregion

        #region Private
        private readonly Dictionary<TimeScaleTypes?, float> timeScales = new();
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            var bulletTimeScale = config.Value.GetConstantFloat(ConstantKeys.BULLET_TIME_SCALE, 0.5f);

            timeScales.Add(TimeScaleTypes.Normal, 1f);
            timeScales.Add(TimeScaleTypes.Paused, 0f);
            timeScales.Add(TimeScaleTypes.BulletTime, bulletTimeScale);

            world.Value.AddChangedListener(this);
        }

        public void Destroy(IEcsSystems systems)
        {
            world.Value.RemoveChangedListener(this);
        }

        public void OnComponentChanged(EcsWorld world, int entity, TimeScaleData? data, EntityChangeTypes changeType)
        {
            if (changeType == EntityChangeTypes.Removed)
                return;

            Debug.Log($"<b>[{GetType().Name}]</b> | {{E:{entity:D8}->C:{data?.GetType().Name}[{changeType}] | V:{data?.Value}}}");

            Time.timeScale = timeScales[data?.Value];
        }
        #endregion
    }
}
