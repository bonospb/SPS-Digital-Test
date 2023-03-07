using UnityEngine;

namespace FreeTeam.BP.Views
{
    public interface IMeshRendererEntityView : IEntityView
    {
        MeshRenderer GetMeshRenderer();
    }
}
