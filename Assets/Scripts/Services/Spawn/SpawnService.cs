using FreeTeam.BP.Configuration;
using FreeTeam.BP.Data;
using FreeTeam.BP.Data.Constants;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Services.App;
using FreeTeam.BP.Services.ObjectPool;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace FreeTeam.BP.Services.Spawn
{
    public class SpawnService : IInitializable
    {
        #region Public
        public bool IsInitialized { get; private set; } = false;
        #endregion

        #region Private
        private ApplicationService _applicationService = null;
        private PoolManager _poolManager = null;
        private IEntityViewFactory _entityViewFactory = null;

        private Configurations _configurations => _applicationService.Configurations;
        private LevelData _levelData => _applicationService.LevelData;

        private readonly Dictionary<string, GameObject> levelPrefabs = new Dictionary<string, GameObject>();
        #endregion

        public SpawnService(ApplicationService applicationService, PoolManager poolManager, IEntityViewFactory entityViewFactory)
        {
            _applicationService = applicationService;
            _poolManager = poolManager;
            _entityViewFactory = entityViewFactory;
        }

        #region Implementation
        public void Initialize() =>
            InitRoutine();
        #endregion

        #region Public methods
        public void SpawnCamera(EcsWorld world)
        {
            if (!IsInitialized)
                throw new System.Exception("SpawnService not initialized!");

            var cameraPrefabPath = _configurations.GetConstantString(ConstantKeys.DEFAULT_CAMERA_PREFAB_PATH);
            var prefab = levelPrefabs[cameraPrefabPath];

            var cameraGO = _poolManager.Spawn(prefab);
            _entityViewFactory.InitEntity(world, cameraGO);
        }

        public void SpawnCharacter(
            EcsWorld world, AvatarConfig config,
            Vector3 position, Quaternion rotation,
            bool isPlayer = false)
        {
            if (!IsInitialized)
                throw new System.Exception("SpawnService not initialized!");

            Debug.Log($"<b>[{GetType().Name}]</b> | Spawn vehicle with ConfigId: <b><i>{config.Id}</i></b>");

            var vehiclePrefab = levelPrefabs[config.PrefabPath];

            var vehicleGO = _poolManager.Spawn(vehiclePrefab, position, rotation);
            var allEntities = _entityViewFactory.InitEntity(world, vehicleGO, config);

            var allArmPlaceEntities = allEntities
                .Where(x => x.Key is IArmPlaceEntityView)
                .Select(x => x.Value);

            var armId = config.AvailableArms?.FirstOrDefault();
            if (!string.IsNullOrEmpty(armId))
            {
                var armPlaceDataPool = world.GetPool<ArmPlaceData>();
                var armPlaceTransformRefPool = world.GetPool<TransformRef>();
                var playerControlPool = world.GetPool<PlayerControl>();

                var armConfig = _configurations.Arms[armId];
                var armPrefab = levelPrefabs[armConfig.PrefabPath];

                foreach (var playerArmPlaceEntity in allArmPlaceEntities)
                {
                    ref var armPlaceData = ref armPlaceDataPool.Get(playerArmPlaceEntity);
                    ref var transformRef = ref armPlaceTransformRefPool.Get(playerArmPlaceEntity);

                    var armGO = _poolManager.Spawn(armPrefab, transformRef.Transform);
                    var armEntity = _entityViewFactory.InitEntity(world, armGO, armConfig).First().Value;

                    if (isPlayer)
                        playerControlPool.Add(armEntity);

                    armPlaceData.ArmEntity = world.PackEntity(armEntity);
                }
            }
        }
        #endregion

        #region Coroutines
        private async void InitRoutine()
        {
            var cameraPrefabPath = _configurations.GetConstantString(ConstantKeys.DEFAULT_CAMERA_PREFAB_PATH);

            var playerAvatarId = _configurations.GetConstantString(ConstantKeys.DEFAULT_PLAYER_AVATAR);
            var playerAvatarConfig = _configurations.Avatars[playerAvatarId];
            var playerPrefabPath = playerAvatarConfig.PrefabPath;

            var levelVehiclesIds = new List<string>
            {
                playerAvatarConfig.Id
            };

            var levelPrefabsPaths = new List<string>
            {
                cameraPrefabPath,
                playerPrefabPath
            };

            var enemiesVehiclesIds = _levelData.LevelConfig.Enemies
                .Select(x => x.Id);
            levelVehiclesIds.AddRange(enemiesVehiclesIds);

            var enemiesVehiclesConfigs = _configurations.Avatars.Values
                .Where(x => enemiesVehiclesIds.Contains(x.Id));

            var enemiesVehiclesPrefabsPath = enemiesVehiclesConfigs
                .Select(x => _configurations.Avatars[x.Id].PrefabPath);
            levelPrefabsPaths.AddRange(enemiesVehiclesPrefabsPath);

            var armsIds = _configurations.Avatars.Values
                .Where(x => levelVehiclesIds.Contains(x.Id))
                .SelectMany(x => x.AvailableArms)
                .Distinct();
            var armsPrefabsPaths = _configurations.Arms.Values
                .Where(x => armsIds.Contains(x.Id))
                .Select(x => x.PrefabPath);
            levelPrefabsPaths.AddRange(armsPrefabsPaths);

            foreach (var path in levelPrefabsPaths)
            {
                Debug.Log($"<b>[{GetType().Name}]</b> | Loading by PrefabPath:<i><{path}></i>");

                var operation = Addressables.LoadAssetAsync<GameObject>(path);
                await operation.Task;

                var prefab = operation.Result;

                levelPrefabs.Add(path, prefab);
                _poolManager.Warm(prefab);
            }

            IsInitialized = true;
        }

        public IEnumerator WaitInitializing()
        {
            yield return new WaitWhile(() => !IsInitialized);
        }
        #endregion
    }
}
