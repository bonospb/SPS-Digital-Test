using UnityEngine;
using Zenject;

namespace FreeTeam.BP.Services.ObjectPool.Factories
{
    public class PoolableObjectFactory : IPoolableObjectFactory
    {
        #region Private
        private readonly DiContainer container = null;
        #endregion

        public PoolableObjectFactory(DiContainer diContainer) =>
            container = diContainer;

        #region Implementation
        public GameObject Instantiate(GameObject gameObject) =>
            container.InstantiatePrefab(gameObject);
        #endregion
    }
}
