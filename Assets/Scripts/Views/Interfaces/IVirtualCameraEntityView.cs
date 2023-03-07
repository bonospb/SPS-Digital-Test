using Cinemachine;

namespace FreeTeam.BP.Views
{
    public interface IVirtualCameraEntityView : IEntityView
    {
        CinemachineVirtualCamera GetVirtualCamera();
    }
}
