using FreeTeam.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.UI.SplashScreens
{
    public class SplashScreensManager : MonoBehaviour
    {
        #region Singleton
        private static SplashScreensManager instance_;
        private static SplashScreensManager Instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = GameObject.FindObjectOfType<SplashScreensManager>();

                if (instance_ == null)
                    Debug.LogError($"No <b>{typeof(SplashScreensManager).FullName}</b> instance set");

                return instance_;
            }
        }
        #endregion

        #region Constants
        private const string SPLASH_SCREEN_PREFABS_PATH = "UI/SplashScreens";

        private const string INITIALIZATION_METHOD_NAME = "Initialization";
        private const string DESTRUCTION_METHOD_NAME = "Destruction";
        #endregion

        #region Private
        private IUIControllerFactory uiControllerFactory = null;

        private readonly Dictionary<System.Type, SplashScreenController> splashScreens = new Dictionary<System.Type, SplashScreenController>();
        #endregion

        #region Public methods
        [Inject]
        public void Construct(IUIControllerFactory factory) =>
            uiControllerFactory = factory;

        public async Task<SplashScreenController> Show(string splashScreenName, object data = null, bool showOnInit = true) =>
            await Show<SplashScreenController>(splashScreenName, data, showOnInit);

        public async Task<T> Show<T>(string splashScreenName, object data = null, bool showOnInit = true) where T : SplashScreenController
        {
            if (splashScreens.TryGetValue(typeof(T), out var splashScreenController))
            {
                if (showOnInit && !splashScreenController.IsShowed)
                    splashScreenController.Show();

                return (T)splashScreenController;
            }

            return await Create<T>(splashScreenName, data, showOnInit);
        }

        public async Task<SplashScreenController> Create(string splashScreenName, object data = null, bool showOnInit = true) =>
            await Create<SplashScreenController>(splashScreenName, data, showOnInit);

        public async Task<T> Create<T>(string splashScreenName, object data = null, bool showOnInit = true) where T : SplashScreenController
        {
            if (string.IsNullOrEmpty(splashScreenName))
            {
                Debug.LogError($"({GetType().Name}) SplashScreenName is null or empty!");

                return null;
            }

            var prefabPath = string.Format(SPLASH_SCREEN_PREFABS_PATH, splashScreenName);

            var prefab = await uiControllerFactory.LoadPrefabAsync($"{SPLASH_SCREEN_PREFABS_PATH}/{splashScreenName}");

            var splashScreenController = uiControllerFactory.Instantiate<T>(prefab, transform);

            splashScreens.Add(splashScreenController.GetType(), splashScreenController);

            splashScreenController.OnDestruct.AddListener(OnDestructionHandler);

            splashScreenController.ShowOnInit = showOnInit;

            splashScreenController.SendMessage(INITIALIZATION_METHOD_NAME, new SplashScreenController.SplashScreenData(data));

            Debug.Log($"({GetType().Name}) SplashScreens count: <b>{splashScreens.Count}</b>");

            return splashScreenController;
        }

        public T Find<T>() where T : SplashScreenController
        {
            if (splashScreens.TryGetValue(typeof(T), out var splashScreenController))
                return (T)splashScreenController;

            return null;
        }

        public void Destroy()
        {
            var splashScreenController = splashScreens.LastOrDefault().Value;

            Destroy(splashScreenController);
        }

        public void Destroy(SplashScreenController splashScreenController)
        {
            if (!splashScreenController)
            {
                Debug.LogError($"({GetType().Name}) Controller is null!");

                return;
            }

            splashScreenController.OnDestruct.RemoveListener(OnDestructionHandler);

            splashScreens.Remove(splashScreenController.GetType());

            GameObject.Destroy(splashScreenController.gameObject);

            Debug.Log($"({GetType().Name}) SplashScreens count: <b>{splashScreens.Count}</b>");
        }

        public void Destroy<T>() where T : SplashScreenController
        {
            if (!splashScreens.TryGetValue(typeof(T), out var splashScreenController))
            {
                Debug.LogError($"({GetType().Name}) Controller \"{typeof(T)}\" is not found!");

                return;
            }

            if (!splashScreenController)
            {
                Debug.LogError($"({GetType().Name}) Controller is null!");

                return;
            }

            splashScreenController.OnDestruct.RemoveListener(OnDestructionHandler);

            splashScreens.Remove(typeof(T));

            GameObject.Destroy(splashScreenController.gameObject);

            Debug.Log($"({GetType().Name}) SplashScreens count: <b>{splashScreens.Count}</b>");
        }

        public void DestroyAll()
        {
            foreach (var splashScreen in splashScreens.Values)
                splashScreen.SendMessage(DESTRUCTION_METHOD_NAME);
        }
        #endregion

        #region Private methods
        private void OnDestructionHandler(IUIController _controller)
        {
            this.Destroy(_controller as SplashScreenController);
        }
        #endregion

        #region Public static methods
        public static async Task<SplashScreenController> ShowSplashScreen(string splashScreenName, object data = null, bool showOnInit = true) =>
            await Instance.Show(splashScreenName, data, showOnInit);

        public static async Task<T> ShowSplashScreen<T>(string splashScreenName, object data = null, bool showOnInit = true) where T : SplashScreenController =>
            await Instance.Show<T>(splashScreenName, data, showOnInit);

        public static async Task<SplashScreenController> CreateSplashScreen(string splashScreenName, object data = null, bool showOnInit = true) =>
            await Instance.Create(splashScreenName, data, showOnInit);

        public static async Task<T> CreateSplashScreen<T>(string splashScreenName, object data = null, bool showOnInit = true) where T : SplashScreenController =>
            await Instance.Create<T>(splashScreenName, data, showOnInit);

        public static T FindSplashScreen<T>() where T : SplashScreenController =>
            Instance.Find<T>();

        public static void DestroySplashScreen() =>
            Instance.Destroy();

        public static void DestroySplashScreen<T>() where T : SplashScreenController =>
            Instance.Destroy<T>();

        public static void DestroyAllSplashScreens() =>
            Instance.DestroyAll();
        #endregion
    }
}