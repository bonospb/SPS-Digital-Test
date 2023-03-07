using FreeTeam.BP.Services.ObjectPool;
using FreeTeam.BP.Editor;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.Services.Effects
{
    public class EffectsService : MonoBehaviour, IInitializable
    {
        #region SerializeFields
        [Foldout("Prefbs", true)]
        [SerializeField] private GameObject trailPrefab = null;
        [SerializeField] private int trailPoolSize = 50;
        [SerializeField] private GameObject hitPrefab = null;
        [SerializeField] private int hitPoolSize = 50;
        [SerializeField] private GameObject explosionPrefab = null;
        [SerializeField] private int explosionPoolSize = 5;
        #endregion

        #region Private
        private PoolManager poolManager = null;
        #endregion

        #region Implementation
        public void Initialize()
        {
            poolManager.Warm(trailPrefab, trailPoolSize);
            poolManager.Warm(hitPrefab, hitPoolSize);
            poolManager.Warm(explosionPrefab, explosionPoolSize);
        }
        #endregion

        #region Public methods
        [Inject]
        public void Construct(PoolManager poolManager) =>
            this.poolManager = poolManager;

        public void Trace(Vector3 startPosition, Vector3 endPosition)
        {
            var shotTrail = poolManager.Spawn(trailPrefab, startPosition, Quaternion.identity).GetComponent<ShotTrail>();
            shotTrail.Emit(startPosition, endPosition);
        }

        public void Hit(Vector3 point, Vector3 normal)
        {
            var hit = poolManager.Spawn(hitPrefab, point, Quaternion.identity).GetComponentInChildren<ParticleSystem>();
            hit.transform.rotation = Quaternion.LookRotation(normal);
            hit.Play();
        }

        public void Explode(Vector3 point)
        {
            var explosion = poolManager.Spawn(explosionPrefab, point, Quaternion.identity).GetComponent<ParticleSystem>();
            explosion.Play();
        }
        #endregion
    }
}