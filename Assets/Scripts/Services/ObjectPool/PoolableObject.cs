using UnityEngine;

namespace FreeTeam.BP.Services.ObjectPool
{
    public class PoolableObject : MonoBehaviour, IPoolableObject
    {
        #region Public
        public event System.Action<GameObject> OnRelease = null;
        #endregion

        #region Unity methods
        private void OnDisable() =>
            OnRelease?.Invoke(gameObject);
        #endregion
    }
}
