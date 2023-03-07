using UnityEngine;

namespace FreeTeam.BP.Services.ObjectPool.Factories
{
    public interface IPoolableObjectFactory
    {
        GameObject Instantiate(GameObject gameObject);
    }
}
