using Assets.Scripts.ECS.Systems.Game;
using FreeTeam.BP.Configuration;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.ECS.Systems;
using FreeTeam.BP.Services.App;
using FreeTeam.BP.Services.Canvas;
using FreeTeam.BP.Services.Effects;
using FreeTeam.BP.Services.Spawn;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.EventSystem;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.ECS
{
    public class EcsStartup : MonoBehaviour
    {
        #region Public
        public bool IsInitialized { get; private set; } = false;
        #endregion

        #region Private
        private EcsWorld ecsWorld = null;

        private EcsSystems initSystems = null;
        private EcsSystems updateSystems = null;
        private EcsSystems fixedUpdateSystem = null;
        private EcsSystems eventSystems = null;

        private EffectsService effectsService = null;
        private WorldSpaceCanvasService worldSpaceCanvasService = null;
        private Configurations configurations = null;
        private SpawnService spawnService = null;
        #endregion

        #region Unity methods
        private void Update()
        {
            updateSystems?.Run();
            eventSystems?.Run();
        }

        private void FixedUpdate() =>
            fixedUpdateSystem?.Run();

        private void OnDestroy()
        {
            eventSystems?.Destroy();
            eventSystems = null;

            fixedUpdateSystem?.Destroy();
            fixedUpdateSystem = null;

            updateSystems?.Destroy();
            updateSystems = null;

            initSystems?.Destroy();
            initSystems = null;

            ecsWorld?.Destroy();
            ecsWorld = null;
        }
        #endregion

        #region Public methods
        [Inject]
        public void Construct(
            EcsWorld world, 
            Configurations config,
            SpawnService spawn,
            EffectsService effects, 
            WorldSpaceCanvasService worldSpaceCanvas)
        {
            ecsWorld = world;
            configurations = config;
            spawnService = spawn;
            effectsService = effects;
            worldSpaceCanvasService = worldSpaceCanvas;

            InitSystems();
        }

        public async void InitSystems()
        {
            if (IsInitialized)
                return;

            eventSystems = new EcsSystems(ecsWorld);
            eventSystems
                .Add(new EventSystem<TimeScaleData>())
                .Add(new EventSystem<GameStateData>())
                .Add(new EventSystem<ProfileData>())
                .Add(new EventSystem<HealthData>())
                .Add(new EventSystem<DeadData>())
                .Add(new EventSystem<DestroyEntityEvent>())

                .Inject()

                .Init();

            initSystems = new EcsSystems(ecsWorld);
            initSystems
                .Add(new MainGameInitSystem())
                .Add(new GameStateSystem())
                .Add(new ProfileSystem())
                .Add(new TimeScaleSystem())
                .Add(new CameraInitSystem())
                .Add(new HealthCheckerSystem())

                .Inject(configurations)

                .Init();

            await spawnService.WaitInit();
        }
        #endregion
    }
}
