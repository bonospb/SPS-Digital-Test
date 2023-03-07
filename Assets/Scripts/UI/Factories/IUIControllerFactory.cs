using FreeTeam.UI;
using System.Threading.Tasks;
using UnityEngine;

namespace FreeTeam.BP.UI
{
    public interface IUIControllerFactory
    {
        Task<GameObject> LoadPrefabAsync(string path);

        T Instantiate<T>(GameObject prefab, Transform parent = null) where T : Component, IUIController;
    }
}
