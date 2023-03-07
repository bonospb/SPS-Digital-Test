using FreeTeam.BP.Services.ObjectPool;
using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class WeaponView : MonoBehaviour, IWeaponEntityView, IWithConfigId, IPoolableObject
    {
        #region SerializeFields
        [SerializeField] private Transform pointTransform = null;
        #endregion

        #region Public
        public event System.Action<GameObject> OnRelease = null;
        #endregion

        #region Unity methods
        private void OnDisable() =>
            OnRelease?.Invoke(gameObject);
        #endregion

        #region Implementation
        public Transform GetTransform() => transform;

        public Transform GetPointTransform() => pointTransform;

        public string ConfigId { get; set; }

        public IArmPlaceEntityView ArmPlaceView { get; private set; }
        #endregion
    }
}
