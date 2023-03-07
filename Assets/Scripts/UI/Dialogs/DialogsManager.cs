using FreeTeam.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.UI.Dialogs
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DialogsManager : MonoBehaviour
    {
        #region Singleton
        private static DialogsManager instance_;
        private static DialogsManager Instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = GameObject.FindObjectOfType<DialogsManager>();

                if (instance_ == null)
                    Debug.LogError($"No <b>{typeof(DialogsManager).FullName}</b> instance set");

                return instance_;
            }
        }
        #endregion

        #region Constants
        private const string DIALOG_PREFABS_PATH = "UI/Dialogs";

        private const string INITIALIZATION_METHOD_NAME = "Initialization";
        private const string DESTRUCTION_METHOD_NAME = "Destruction";
        #endregion

        #region Private
        private IUIControllerFactory uiControllerFactory = null;

        private readonly Dictionary<System.Type, DialogController> dialogs = new Dictionary<System.Type, DialogController>();

        private CanvasGroup canvasGroup_ = null;
        private CanvasGroup CanvasGroup
        {
            get
            {
                if (!canvasGroup_)
                {
                    canvasGroup_ = this.gameObject.GetComponent<CanvasGroup>();
                    if (!canvasGroup_)
                        canvasGroup_ = this.gameObject.AddComponent<CanvasGroup>();
                }

                return canvasGroup_;
            }
        }
        #endregion

        #region Public methods
        [Inject]
        public void Construct(IUIControllerFactory factory) =>
            uiControllerFactory = factory;

        public async Task<DialogController> Create(string dialogName, object data = null) =>
            await Create<DialogController>(dialogName, data);

        public async Task<T> Create<T>(string dialogName, object data = null) where T : DialogController
        {
            if (dialogs.TryGetValue(typeof(T), out var dialogController))
                return (T)dialogController;

            if (string.IsNullOrEmpty(dialogName))
            {
                Debug.LogError($"({GetType().Name}) DialogName is null or empty!");

                return null;
            }

            var prefab = await uiControllerFactory.LoadPrefabAsync($"{DIALOG_PREFABS_PATH}/{dialogName}");

            dialogController = uiControllerFactory.Instantiate<T>(prefab, transform);

            dialogs.Add(dialogController.GetType(), dialogController);

            Debug.Log($"({GetType().Name}) Dialogs count: <b>{dialogs.Count}</b>");

            dialogController.OnDestruct.AddListener(OnDestructionHandler);
            dialogController.SendMessage(INITIALIZATION_METHOD_NAME, new DialogController.DialogData(data));

            foreach (var dialog in dialogs)
            {
                if (dialog.Value != dialogController)
                    dialog.Value.IsActive = false;
            }

            return (T)dialogController;
        }

        public T Find<T>() where T : DialogController
        {
            if (dialogs.TryGetValue(typeof(T), out var dialogController))
                return (T)dialogController;

            return null;
        }

        public void Close()
        {
            var dialogController = dialogs.Values.Last();

            Close(dialogController);
        }

        public void Close(DialogController _dialogController)
        {
            if (!_dialogController)
            {
                Debug.LogWarning($"({GetType().Name}) Controller is null!");

                return;
            }

            _dialogController.Close();
        }

        public void Destroy<T>() where T : DialogController
        {
            if (!dialogs.TryGetValue(typeof(T), out var dialogController))
            {
                Debug.LogError($"({GetType().Name}) Controller \"{typeof(T)}\" is not found!");

                return;
            }

            if (!dialogController)
            {
                Debug.LogWarning($"({GetType().Name}) Controller is null!");

                return;
            }

            Destroy(dialogController);
        }

        public void Destroy(DialogController dialogController)
        {
            dialogController.OnDestruct.RemoveListener(OnDestructionHandler);

            var t = dialogController.GetType();

            dialogs.Remove(t);

            var dialog = dialogs.Values.LastOrDefault();
            if (dialog)
                dialog.IsActive = true;

            Debug.Log($"({GetType().Name}) Dialogs count: <b>{dialogs.Count}</b>");
        }

        public void DestroyAll()
        {
            List<DialogController> destroedDialogs = dialogs.Values.ToList();

            foreach (var dialogController in destroedDialogs)
                dialogController.SendMessage(DESTRUCTION_METHOD_NAME);
        }
        #endregion

        #region Private methods
        private void OnDestructionHandler(IUIController _controller) => Destroy(_controller as DialogController);
        #endregion

        #region Public static methods
        public static async Task<DialogController> CreateDialog(string dialogName, object data = null) =>
            await Instance.Create(dialogName, data);

        public static async Task<T> CreateDialog<T>(string dialogName, object data = null) where T : DialogController =>
             await Instance.Create<T>(dialogName, data);

        public static T FindDialog<T>() where T : DialogController =>
            Instance.Find<T>();

        public static void CloseDialog() =>
            Instance.Close();

        public static void CloseDialog(DialogController _dialogController) =>
            Instance.Close(_dialogController);

        public static void DestroyDialog(DialogController dialogController) =>
            Instance.Destroy(dialogController);

        public static void DestroyDialog<T>() where T : DialogController =>
            Instance.Destroy<T>();

        public static void DestroyAllDialogs() =>
            Instance.DestroyAll();

        public static int DialogsCount =>
            Instance.dialogs.Count;
        #endregion
    }
}