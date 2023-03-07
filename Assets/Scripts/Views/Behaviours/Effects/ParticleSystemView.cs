using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class ParticleSystemView : MonoBehaviour, IParticleSystemEntityView
    {
        #region SerializeFields
        [SerializeField] private new ParticleSystem particleSystem = null;
        #endregion

        #region Implementation
        public ParticleSystem GetParticleSystem() => particleSystem;

        public Transform GetTransform() => transform;
        #endregion
    }
}
