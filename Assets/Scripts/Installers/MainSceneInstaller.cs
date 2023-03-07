using FreeTeam.BP.Configuration;
using FreeTeam.BP.Services.App;
using FreeTeam.BP.Services.Canvas;
using FreeTeam.BP.Services.Effects;
using FreeTeam.BP.Services.ObjectPool;
using FreeTeam.BP.Services.ObjectPool.Factories;
using FreeTeam.BP.Services.Spawn;
using FreeTeam.BP.UI;
using FreeTeam.BP.UI.Dialogs;
using FreeTeam.BP.UI.Screens;
using FreeTeam.BP.UI.SplashScreens;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace FreeTeam.BP.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        #region Public methods
        public override void InstallBindings()
        {
            BindUIManagers();

            BindConfig();

            BindObjectPoolService();
            BindSpawnService();
            BindEffectService();
            BindWorldSpaceCanvasService();

            BindUIControllerFactory();
            BindPoolableObjectFactory();
            BindEntityViewFactory();

            BindEcsWorld();

            BindInstallerInterfaces();
        }
        #endregion

        #region Private methods
        private void BindUIManagers()
        {
            Container
                .Bind<ScreensManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<DialogsManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<SplashScreensManager>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        private void BindUIControllerFactory()
        {
            Container
                .Bind<IUIControllerFactory>()
                .To<UIControllerFactory>()
                .AsSingle();
        }

        private void BindConfig()
        {
            var config = LoadConfigurations();
            Container
                .Bind<Configurations>()
                .FromInstance(config)
                .AsSingle();
        }

        private void BindObjectPoolService()
        {
            Container
                .Bind<PoolManager>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        private void BindSpawnService()
        {
            Container
                .Bind<SpawnService>()
                .AsSingle()
                .NonLazy();
        }

        private void BindEffectService()
        {
            Container
                .BindInterfacesAndSelfTo<EffectsService>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        private void BindPoolableObjectFactory()
        {
            Container
                .Bind<IPoolableObjectFactory>()
                .To<PoolableObjectFactory>()
                .AsSingle();
        }

        private void BindWorldSpaceCanvasService()
        {
            Container
                .Bind<WorldSpaceCanvasService>()
                .FromComponentsInHierarchy()
                .AsSingle();
        }

        private void BindEcsWorld()
        {
            Container
                .Bind<EcsWorld>()
                .AsSingle()
                .NonLazy();
        }

        private void BindEntityViewFactory()
        {
            Container
                .Bind<IEntityViewFactory>()
                .To<EntityViewFactory>()
                .AsSingle();
        }

        private void BindInstallerInterfaces()
        {
            Container
                .BindInterfacesAndSelfTo<ApplicationService>()
                .FromComponentInHierarchy(this)
                .AsSingle();
        }

        private Configurations LoadConfigurations()
        {
            var op = Addressables.LoadAssetAsync<TextAsset>(@"Configs/config.txt");
            TextAsset ta = op.WaitForCompletion();
            var json = ta.text;

            return Configurations.Load(json);
        }
        #endregion
    }
}
