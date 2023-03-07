using Cinemachine;
using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class VirtualCameraView : MonoBehaviour, IVirtualCameraEntityView
    {
        #region SerializeFields
        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
        #endregion

        #region Implementation
        public Transform GetTransform() => transform;
        public CinemachineVirtualCamera GetVirtualCamera() => virtualCamera;
        #endregion
    }
}
