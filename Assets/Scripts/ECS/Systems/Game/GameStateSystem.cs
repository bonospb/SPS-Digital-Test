using FreeTeam.BP.Common;
using FreeTeam.BP.Configuration;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.ECS.Systems.FSM;
using FreeTeam.BP.FSM;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.EventSystem;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems
{
    public class GameStateSystem :
        StateMachine,
        IEcsPreInitSystem,
        IEcsInitSystem,
        IEcsDestroySystem,
        EventSystem<GameStateData>.IComponentChangedListener
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsCustomInject<Configurations> config = default;
        #endregion

        #region Private
        private readonly Dictionary<GameStateTypes?, IState> states = new();
        #endregion

        #region Implementation
        public void PreInit(IEcsSystems systems)
        {
            states.Add(GameStateTypes.Init, new InitState(this, world.Value, config.Value));
            states.Add(GameStateTypes.Stoped, new StopedState(this, world.Value));
            states.Add(GameStateTypes.Played, new PlayedState(this, world.Value));
            states.Add(GameStateTypes.Paused, new PausedState(this, world.Value));
        }

        public void Init(IEcsSystems systems) =>
            world.Value.AddChangedListener(this);

        public void Destroy(IEcsSystems systems) =>
            world.Value.RemoveChangedListener(this);

        public void OnComponentChanged(EcsWorld world, int entity, GameStateData? data, EntityChangeTypes changeType)
        {
            if (changeType.HasFlag(EntityChangeTypes.Removed))
                return;

            Debug.Log($"E{entity:D8}:[{changeType}]->{data?.GetType().Name}->{data?.Value}");

            if (states.TryGetValue(data?.Value, out var state))
                ChangeState(state);
        }
        #endregion
    }
}
