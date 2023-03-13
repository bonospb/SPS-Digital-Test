using Assets.Scripts.ECS.Systems.Game;
using FreeTeam.BP.Configuration;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.ECS.Systems;
using FreeTeam.BP.Services.Canvas;
using FreeTeam.BP.Services.Effects;
using FreeTeam.BP.Services.Spawn;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.EventSystem;
using Leopotam.EcsLite.ExtendedSystems;
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

        private Configurations configurations = null;
        private EffectsService effectsService = null;
        private SpawnService spawnService = null;
        private WorldSpaceCanvasService worldSpaceCanvasService = null;
        private IEntityViewFactory entityViewFactory = null;
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
            EcsWorld ecsWorld,
            Configurations configurations,
            SpawnService spawnService,
            EffectsService effectsService,
            WorldSpaceCanvasService worldSpaceCanvasService,
            IEntityViewFactory entityViewFactory)
        {
            this.ecsWorld = ecsWorld;
            this.configurations = configurations;
            this.spawnService = spawnService;
            this.effectsService = effectsService;
            this.worldSpaceCanvasService = worldSpaceCanvasService;
            this.entityViewFactory = entityViewFactory;

            InitSystems();
        }

        public async void InitSystems()
        {
            if (IsInitialized)
                return;

            //--------------------------------------------//

            eventSystems = new EcsSystems(ecsWorld);
            eventSystems
                .Add(new EventSystem<TimeScaleData>())
                .Add(new EventSystem<GameStateData>())
                .Add(new EventSystem<ProfileData>())
                .Add(new EventSystem<HealthData>())
                .Add(new EventSystem<DeadData>())
                .Add(new EventSystem<LoadLevelEvent>())
                .Add(new EventSystem<DestroyEntityEvent>())

                .Inject()

                .Init();

            //--------------------------------------------//

            initSystems = new EcsSystems(ecsWorld);
            initSystems
                .Add(new MainGameInitSystem())
                .Add(new GameStateSystem())
                .Add(new TimeScaleSystem())
                .Add(new CameraInitSystem())
                .Add(new HealthCheckerSystem())

                .Inject(configurations)

                .Init();

            //--------------------------------------------//

            fixedUpdateSystem = new EcsSystems(ecsWorld);
            fixedUpdateSystem

                .Inject(configurations)
                .Inject(effectsService)

                .Init();

            //--------------------------------------------//

            updateSystems = new EcsSystems(ecsWorld);
            updateSystems
                .Add(new ProfileSystem())
                .Add(new LoadLevelSystem())
                .Add(new DeadSystem())
                .Add(new ApplyDamageSystem())
                .Add(new ApplyHealSystem())
                .Add(new DestroyEntitySystem())

#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif

                .DelHere<DamageData>()
                .DelHere<HealData>()
                .DelHere<HitData>()
                .DelHere<LoadLevelEvent>()
                .DelHere<DestroyEntityEvent>()

                .Inject(configurations)
                .Inject(spawnService)
                .Inject(effectsService)
                .Inject(worldSpaceCanvasService)

                .Init();

            //--------------------------------------------//

            await spawnService.WaitInit();
        }
        #endregion
    }
}
