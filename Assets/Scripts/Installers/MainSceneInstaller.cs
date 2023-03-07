using FreeTeam.BP.Configuration;
using FreeTeam.BP.Services.App;
using FreeTeam.BP.Services.Canvas;
using FreeTeam.BP.Services.ObjectPool;
using FreeTeam.BP.Services.ObjectPool.Factories;
using FreeTeam.BP.UI;
using FreeTeam.BP.UI.Dialogs;
using FreeTeam.BP.UI.Screens;
using FreeTeam.BP.UI.SplashScreens;
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
            BindWorldSpaceCanvasService();

            BindUIControllerFactory();
            BindPoolableObjectFactory();

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

        private void BindInstallerInterfaces()
        {
            Container
                .BindInterfacesAndSelfTo<ApplicationService>()
                .FromComponentInHierarchy(this)
                .AsSingle();
        }

        private Configurations LoadConfigurations()
        {
            var op = Addressables.LoadAssetAsync<TextAsset>(@"Configs/config");
            TextAsset ta = op.WaitForCompletion();
            var json = ta.text;

            return Configurations.Load(json);
        }
        #endregion
    }
}
