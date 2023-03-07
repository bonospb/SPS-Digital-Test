using UnityEngine;

namespace FreeTeam.BP.Services.ObjectPool
{
    public interface IPoolableObject
    {
        event System.Action<GameObject> OnRelease;
    }
}
