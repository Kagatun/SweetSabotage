using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Spawner
{
    public abstract class SpawnerListObjects<T> : MonoBehaviour, IPoolAdder<T>
        where T : MonoBehaviour
    {
        [SerializeField] private List<T> _prefabs;
        [SerializeField] private List<int> _numbers;

        private ObjectPool<T> _pool;
        private List<T> _allObjects;
        private List<T> _inactiveObjects;

        private void Start()
        {
            _allObjects = new List<T>();
            _inactiveObjects = new List<T>();

            _pool = new ObjectPool<T>(CreateObject, OnGet, OnRelease, OnDestroyObject, false);

            InitializePool();
        }

        private void InitializePool()
        {
            List<T> objectsToAdd = new List<T>();

            for (int j = 0; j < _prefabs.Count; j++)
            {
                for (int i = 0; i < _numbers[j]; i++)
                {
                    T newObject = CreateSpecificObject(_prefabs[j]);
                    objectsToAdd.Add(newObject);
                }
            }

            Shuffle(objectsToAdd);

            foreach (var obj in objectsToAdd)
            {
                _pool.Release(obj);
                _allObjects.Add(obj);
                _inactiveObjects.Add(obj);
            }
        }

        private T CreateSpecificObject(T prefab) =>
            Instantiate(prefab);

        private void Shuffle(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        public void AddToPool(T obj)
        {
            _pool.Release(obj);
            _inactiveObjects.Add(obj);
        }

        protected T Get()
        {
            if (_inactiveObjects.Count == 0)
            {
                T newObject = CreateObject();
                _pool.Release(newObject);
                _inactiveObjects.Add(newObject);
            }

            int randomIndex = Random.Range(0, _inactiveObjects.Count);
            T randomObject = _inactiveObjects[randomIndex];
            OnGet(randomObject);
            _inactiveObjects.Remove(randomObject);

            return randomObject;
        }

        protected virtual T CreateObject()
        {
            List<T> activePrefabs = new List<T>();

            for (int i = 0; i < _prefabs.Count; i++)
                activePrefabs.Add(_prefabs[i]);

            if (activePrefabs.Count == 0)
                return null;

            int randomIndex = Random.Range(0, activePrefabs.Count);

            return Instantiate(activePrefabs[randomIndex]);
        }

        protected virtual void OnGet(T obj) =>
            obj.gameObject.SetActive(true);

        protected virtual void OnRelease(T obj) =>
            obj.gameObject.SetActive(false);

        protected virtual void OnDestroyObject(T obj) =>
            Destroy(obj.gameObject);
    }
}