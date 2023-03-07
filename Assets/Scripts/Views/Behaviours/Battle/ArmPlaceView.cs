using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class ArmPlaceView : MonoBehaviour, IArmPlaceEntityView
    {
        #region Implementation
        public Transform GetTransform() => transform;
        #endregion
    }
}
