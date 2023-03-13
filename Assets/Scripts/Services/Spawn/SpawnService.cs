using FreeTeam.BP.Configuration;
using FreeTeam.BP.Data.Constants;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Services.ObjectPool;
using FreeTeam.BP.Utils;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FreeTeam.BP.Services.Spawn
{
    public class SpawnService
    {
        #region Public
        public bool IsInitialized { get; private set; } = false;
        #endregion

        #region Private
        private Configurations configurations = null;
        private PoolManager poolManager = null;
        private IEntityViewFactory entityViewFactory = null;

        private readonly Dictionary<string, GameObject> levelPrefabs = new Dictionary<string, GameObject>();
        #endregion

        public SpawnService(
            Configurations configurations,
            PoolManager poolManager,
            IEntityViewFactory entityViewFactory)
        {
            this.configurations = configurations;
            this.poolManager = poolManager;
            this.entityViewFactory = entityViewFactory;
        }

        #region Implementation
        public void Initialize(LevelConfig levelConfig) =>
            PreloadPrefabs(levelConfig);
        #endregion

        #region Public methods
        public void SpawnCamera(EcsWorld world)
        {
            if (!IsInitialized)
                throw new System.Exception("SpawnService not initialized!");

            var cameraPrefabPath = configurations.GetConstantString(ConstantKeys.DEFAULT_CAMERA_PREFAB_PATH);
            var prefab = levelPrefabs[cameraPrefabPath];

            var cameraGO = poolManager.Spawn(prefab);
            entityViewFactory.InitEntity(world, cameraGO);
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

            var vehicleGO = poolManager.Spawn(vehiclePrefab, position, rotation);
            var allEntities = entityViewFactory.InitEntity(world, vehicleGO, config);

            var allArmPlaceEntities = allEntities
                .Where(x => x.Key is IArmPlaceEntityView)
                .Select(x => x.Value);

            var armId = config.Arm;
            if (!string.IsNullOrEmpty(armId))
            {
                var armPlaceDataPool = world.GetPool<ArmPlaceData>();
                var armPlaceTransformRefPool = world.GetPool<TransformRef>();
                var playerControlPool = world.GetPool<PlayerControl>();

                var armConfig = configurations.Arms[armId];
                var armPrefab = levelPrefabs[armConfig.PrefabPath];

                foreach (var playerArmPlaceEntity in allArmPlaceEntities)
                {
                    ref var armPlaceData = ref armPlaceDataPool.Get(playerArmPlaceEntity);
                    ref var transformRef = ref armPlaceTransformRefPool.Get(playerArmPlaceEntity);

                    var armGO = poolManager.Spawn(armPrefab, transformRef.Transform);
                    var armEntity = entityViewFactory.InitEntity(world, armGO, armConfig).First().Value;

                    if (isPlayer)
                        playerControlPool.Add(armEntity);

                    armPlaceData.ArmEntity = world.PackEntity(armEntity);
                }
            }
        }

        public async Task WaitInit() =>
            await TaskUtils.WaitUntil(() => IsInitialized);
        #endregion

        #region Private methods
        private async void PreloadPrefabs(LevelConfig levelConfig)
        {
            var cameraPrefabPath = configurations.GetConstantString(ConstantKeys.DEFAULT_CAMERA_PREFAB_PATH);

            var playerAvatarId = configurations.GetConstantString(ConstantKeys.DEFAULT_PLAYER_AVATAR);
            var playerAvatarConfig = configurations.Avatars[playerAvatarId];
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

            var enemiesVehiclesIds = levelConfig.Enemies
                .Select(x => x.Id);
            levelVehiclesIds.AddRange(enemiesVehiclesIds);

            var enemiesVehiclesConfigs = configurations.Avatars.Values
                .Where(x => enemiesVehiclesIds.Contains(x.Id));

            var enemiesVehiclesPrefabsPath = enemiesVehiclesConfigs
                .Select(x => configurations.Avatars[x.Id].PrefabPath);
            levelPrefabsPaths.AddRange(enemiesVehiclesPrefabsPath);

            var armsIds = configurations.Avatars.Values
                .Where(x => levelVehiclesIds.Contains(x.Id))
                .Select(x => x.Arm)
                .Distinct();
            var armsPrefabsPaths = configurations.Arms.Values
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
                poolManager.Warm(prefab);
            }

            IsInitialized = true;
        }
        #endregion
    }
}
