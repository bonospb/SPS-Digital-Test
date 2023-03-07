using FreeTeam.BP.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BP.ECS.Systems
{
    public class MainGameInitSystem : IEcsInitSystem, IEcsDestroySystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            world.Value.AddUnique<GameStateData>().Value = Common.GameStateTypes.Init;
            world.Value.AddUnique<TimeScaleData>().Value = Common.TimeScaleTypes.Normal;
        }

        public void Destroy(IEcsSystems systems)
        {
            world.Value.DelUnique<TimeScaleData>();
            world.Value.DelUnique<GameStateData>();
        }
        #endregion
    }
}
