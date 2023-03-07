using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class CollisionView : MonoBehaviour, ICollisionEntityView
    {
        #region SerializeFields
        [SerializeField] private new Collider collider = null;
        #endregion

        #region Implementation
        public Transform GetTransform() => transform;

        public Collider GetCollider() => collider;
        #endregion
    }
}
