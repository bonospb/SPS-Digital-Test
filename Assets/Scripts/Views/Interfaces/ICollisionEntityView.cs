using UnityEngine;

namespace FreeTeam.BP.Views
{
    public interface ICollisionEntityView : IEntityView
    {
        Collider GetCollider();
    }
}
