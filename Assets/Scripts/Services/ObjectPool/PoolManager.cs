using FreeTeam.BP.Services.ObjectPool.Factories;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.Services.ObjectPool
{
    public class PoolManager : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private bool logStatus = false;
        [SerializeField] private Transform root = null;
        #endregion

        #region Private
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();

        private bool dirty = false;

        private IPoolableObjectFactory poolableObjectFactory = null;
        private readonly LinkedList<IPoolableObject> poolableObjects = new();
        #endregion

        #region Unity methods
        private void Update()
        {
            if (logStatus && dirty)
            {
                PrintStatus();

                dirty = false;
            }
        }

        private void OnDestroy()
        {
            foreach (var poolableObject in poolableObjects)
                poolableObject.OnRelease -= OnReleaseHandler;

            poolableObjects.Clear();
        }
        #endregion

        #region Public methods
        [Inject]
        public void Construct(IPoolableObjectFactory factory) =>
            poolableObjectFactory = factory;

        public void Warm(GameObject prefab, int size = 1)
        {
            if (prefabLookup.ContainsKey(prefab))
                throw new System.Exception("Pool for prefab " + prefab.name + " has already been created");

            prefabLookup[prefab] = new ObjectPool<GameObject>(() => InstantiatePrefab(prefab), size);

            dirty = true;
        }

        public GameObject Spawn(GameObject prefab) =>
            Spawn(prefab, Vector3.zero, Quaternion.identity);

        public GameObject Spawn(GameObject prefab, Transform parent) =>
            Spawn(prefab, parent.position, parent.rotation, parent);

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (!prefabLookup.ContainsKey(prefab))
                Warm(prefab);

            var pool = prefabLookup[prefab];

            var clone = pool.GetItem();
            clone.transform.SetParent(parent ?? root);
            clone.transform.SetPositionAndRotation(position, rotation);
            clone.SetActive(true);

            instanceLookup.Add(clone, pool);
            dirty = true;

            return clone;
        }

        public void Release(GameObject clone)
        {
            if (enabled)
            {
                clone.SetActive(false);
                clone.transform.SetParent(root);

                if (instanceLookup.ContainsKey(clone))
                {
                    instanceLookup[clone].ReleaseItem(clone);
                    instanceLookup.Remove(clone);
                    dirty = true;
                }
            }
        }

        public void PrintStatus()
        {
            foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in prefabLookup)
                Debug.Log($"Object Pool for Prefab: {keyVal.Key.name} In Use: {keyVal.Value.CountUsedItems} Total {keyVal.Value.Count}");
        }
        #endregion

        #region Private methods
        private GameObject InstantiatePrefab(GameObject prefab)
        {
            var go = poolableObjectFactory.Instantiate(prefab);
            go.SetActive(false);

            if (!go.TryGetComponent<IPoolableObject>(out var poolableObject))
                poolableObject = go.AddComponent<PoolableObject>();

            poolableObject.OnRelease += OnReleaseHandler;

            poolableObjects.AddLast(poolableObject);

            return go;
        }

        private void OnReleaseHandler(GameObject clone) =>
            Release(clone);
        #endregion
    }
}