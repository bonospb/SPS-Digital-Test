using FreeTeam.BP.Services.ObjectPool;
using FreeTeam.BP.UI.Panels;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.Services.Canvas
{
    public class WorldSpaceCanvasService : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private GameObject healthBarPrefab = null;
        [SerializeField] private UnityEngine.Canvas canvas = null;
        #endregion

        #region Private
        private PoolManager poolManager = null;
        #endregion

        #region Public methods
        [Inject]
        public void Construct(PoolManager manager) =>
            poolManager = manager;

        public HealthBarPanel InstantiateHealthBarPanel() =>
            poolManager.Spawn(healthBarPrefab, Vector3.zero, Quaternion.identity, canvas.transform).GetComponent<HealthBarPanel>();
        #endregion
    }
}
