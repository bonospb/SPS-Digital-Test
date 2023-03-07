using FreeTeam.UI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace FreeTeam.BP.UI
{
    public class UIControllerFactory : IUIControllerFactory
    {
        #region Private
        private readonly DiContainer container = null;
        #endregion

        public UIControllerFactory(DiContainer diContainer) =>
            container = diContainer;

        #region Implementation
        public async Task<GameObject> LoadPrefabAsync(string path)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path + ".prefab");
            await handle.Task;
            return handle.Result;
        }

        public T Instantiate<T>(GameObject prefab, Transform parent = null) where T : Component, IUIController =>
            container.InstantiatePrefabForComponent<T>(prefab, parent);
        #endregion
    }
}
