using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MK.Pooling
{
    public class SimplePool<T> where T : class, IPoolObject
    {
        private ObjectPool<T> _pool;
        private IPoolObject _template;

        public SimplePool(T template)
        {
            _pool = new ObjectPool<T>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
            _template = template;
        }

        public T GetItem()
        {
            return _pool.Get();
        }

        public void ReleaseItem(T item)
        {
            _pool.Release(item);
        }

        T CreatePooledItem()
        {
            var obj = (T)_template.CreateNew();
            obj.DestroyAction = () => _pool.Release(obj);
            obj.Reset();
            return obj;
        }

        void OnReturnedToPool(T obj)
        {
            obj.SetActive(false);
            obj.Reset();
        }

        void OnTakeFromPool(T obj)
        {
            obj.SetActive(true);
        }

        void OnDestroyPoolObject(T obj)
        {
            obj.OnOverflowDestroy();
        }
    }
}