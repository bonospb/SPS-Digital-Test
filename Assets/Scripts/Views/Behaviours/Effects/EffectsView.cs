using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class EffectsView : MonoBehaviour, IEffectsEntityView
    {
        #region SerializeFields
        [SerializeField] private ParticleSystemView[] particleSystemViews = null;
        [SerializeField] private TrailRendererView[] trailRendererViews = null;
        #endregion

        #region Implementation
        public Transform GetTransform() =>
            transform;

        public IParticleSystemEntityView[] ParticleSystemViews => particleSystemViews;
        public ITrailRendererEntityView[] TrailRendererViews => trailRendererViews;
        #endregion
    }
}
