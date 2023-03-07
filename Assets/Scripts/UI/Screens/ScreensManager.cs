using FreeTeam.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.UI.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreensManager : MonoBehaviour
    {
        #region Singleton
        private static ScreensManager instance_;
        private static ScreensManager Instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = GameObject.FindObjectOfType<ScreensManager>();

                if (instance_ == null)
                    Debug.LogError($"No <b>{typeof(ScreensManager).Name}</b> instance set");

                return instance_;
            }
        }
        #endregion

        #region Constants
        private const string SCREEN_PREFABS_PATH = "UI/Screens";

        private const string INITIALIZATION_METHOD_NAME = "Initialization";
        private const string DESTRUCTION_METHOD_NAME = "Destruction";
        #endregion

        #region Private
        private IUIControllerFactory uiControllerFactory = null;

        private readonly List<ScreenController> screens = new List<ScreenController>();

        private CanvasGroup canvasGroup_ = null;
        private CanvasGroup CanvasGroup
        {
            get
            {
                if (!canvasGroup_)
                {
                    canvasGroup_ = gameObject.GetComponent<CanvasGroup>();
                    if (!canvasGroup_)
                        canvasGroup_ = gameObject.AddComponent<CanvasGroup>();
                }

                return canvasGroup_;
            }
        }
        #endregion

        #region Public methods
        [Inject]
        public void Construct(IUIControllerFactory factory) =>
            uiControllerFactory = factory;

        public async Task<ScreenController> Create(string screenName, string prevScreenName = null, object data = null) =>
            await Create<ScreenController>(screenName, prevScreenName, data);

        public async Task<T> Create<T>(string screenName, string prevScreenName = null, object data = null) where T : ScreenController
        {
            if (string.IsNullOrEmpty(screenName))
            {
                Debug.LogError($"({GetType().Name}) ScreenName is null or empty!");

                return null;
            }

            var prefab = await uiControllerFactory.LoadPrefabAsync($"{SCREEN_PREFABS_PATH}/{screenName}");

            var screenController = uiControllerFactory.Instantiate<T>(prefab, transform);

            screens.Add(screenController);

            screenController.OnDestruct.AddListener(OnDestructionHandler);
            screenController.SendMessage(INITIALIZATION_METHOD_NAME, new ScreenController.ScreenData(data, prevScreenName));

            Debug.Log($"({GetType().Name}) Screens count: <b>{screens.Count}</b>");

            return screenController;
        }

        public T FindFirst<T>() where T : ScreenController => screens.OfType<T>().FirstOrDefault();

        public T FindLast<T>() where T : ScreenController => screens.OfType<T>().LastOrDefault();

        public void Destroy()
        {
            var screenController = screens.Last();

            Destroy(screenController);
        }

        public void Destroy(ScreenController _screenController)
        {
            if (!_screenController)
            {
                Debug.LogWarning($"({GetType().Name}) Controller is null!");

                return;
            }

            _screenController.OnDestruct.RemoveListener(OnDestructionHandler);

            screens.Remove(_screenController);

            Debug.Log($"({GetType().Name}) Screens count: <b>{screens.Count}</b>");
        }

        public void DestroyAll(ScreenController[] ignoreList = null)
        {
            List<ScreenController> destroedScreens = new List<ScreenController>();

            foreach (var screenController in screens)
            {
                if (ignoreList != null && ignoreList.Contains(screenController))
                    continue;

                destroedScreens.Add(screenController);
            }

            foreach (var screenController in destroedScreens)
                screenController.SendMessage(DESTRUCTION_METHOD_NAME);
        }
        #endregion

        #region Private methods
        private void OnDestructionHandler(IUIController _controller) => Destroy(_controller as ScreenController);
        #endregion

        #region Public static methods
        public static async Task<ScreenController> CreateScreen(string screenName, string prevScreenName = null, object data = null) =>
            await Instance.Create(screenName, prevScreenName, data);

        public static async Task<T> CreateScreen<T>(string screenName, string prevScreenName = null, object data = null) where T : ScreenController =>
            await Instance.Create<T>(screenName, prevScreenName, data);

        public static T FindFirstScreen<T>() where T : ScreenController =>
            Instance.FindFirst<T>();

        public static T FindLastScreen<T>() where T : ScreenController =>
            Instance.FindLast<T>();

        public static void DestroyScreen() =>
            Instance.Destroy();

        public static void DestroyScreen(ScreenController _screenController) =>
            Instance.Destroy(_screenController);

        public static void DestroyAllScreens(ScreenController[] ignoreList = null) =>
            Instance.DestroyAll(ignoreList);

        public static int ScreensCount =>
            Instance.screens.Count;
        #endregion
    }
}