using UnityEngine;

namespace FreeTeam.BP.Views
{
    public interface ITrailRendererEntityView : IEntityView
    {
        TrailRenderer GetTrailRenderer();
    }
}
