using FreeTeam.BP.Behaviours.Converters;
using FreeTeam.BP.Configuration;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Services.Spawn;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace FreeTeam.BP.ECS.Systems
{
    public class LoadLevelSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsWorldInject world = default;

        private readonly EcsCustomInject<Configurations> config = default;
        private readonly EcsCustomInject<IEntityViewFactory> entityViewFactory = default;
        private readonly EcsCustomInject<SpawnService> spawnService = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            if (!world.Value.HasUnique<LoadLevelEvent>())
                return;

            LoadLevel();
            InitializeLevel();
        }
        #endregion

        #region Private methods
        private async void LoadLevel()
        {
            var levelData = world.Value.GetUnique<LevelData>();
            var levelConfig = config.Value.Levels[levelData.LevelId];

            var operation = Addressables.LoadSceneAsync(levelConfig.Scene, LoadSceneMode.Additive);
            await operation.Task;

            var sceneInstance = operation.Result;

            world.Value.ReplaceUnique<SceneInstanceRef>().Value = sceneInstance;
        }

        private void InitializeLevel()
        {
            //var viewConverters = GameObject
            //    .FindObjectsOfType<MonoBehaviour>()
            //    .OfType<IViewConverter>()
            //    .OrderBy(x => x.Priority);

            //var entities = new Dictionary<GameObject, int>();
            //foreach (var converter in viewConverters)
            //    converter.Convert(world.Value, entities);
        }
        #endregion
    }
}
