using System;

namespace MK.Pooling
{
    public interface IPoolObject
    {
        Action DestroyAction { get; set; }
        IPoolObject CreateNew();
        void SetActive(bool v);
        void Reset();
        void OnOverflowDestroy();
    }
}