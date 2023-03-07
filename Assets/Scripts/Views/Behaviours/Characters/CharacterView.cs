using FreeTeam.BP.Services.ObjectPool;
using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class CharacterView : MonoBehaviour, ICharacterEntityView, IWithConfigId, IPoolableObject
    {
        #region SerializeFields
        [SerializeField] private string configId = null;
        [Space(10)]
        [SerializeField] private ArmPlaceView[] armPlaceViews = null;
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

        public IArmPlaceEntityView[] ArmPlaceViews => armPlaceViews;

        public string ConfigId
        {
            get => configId;
            set
            {
                if (configId == value)
                    return;

                configId = value;
            }
        }
        #endregion
    }
}
