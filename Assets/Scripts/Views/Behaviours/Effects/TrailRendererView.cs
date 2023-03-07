using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class TrailRendererView : MonoBehaviour, ITrailRendererEntityView
    {
        #region SerializeFields
        [SerializeField] private TrailRenderer trailRenderer = null;
        #endregion

        #region Implementation
        public TrailRenderer GetTrailRenderer() => trailRenderer;

        public Transform GetTransform() => transform;
        #endregion
    }
}
