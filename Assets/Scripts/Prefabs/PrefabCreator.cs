using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class PrefabCreator
{
    public static async UniTask<GameObject> GetInstance(string name, Transform parent)
    {
        return await Addressables.InstantiateAsync(name, parent.position, Quaternion.identity, parent).Task;
    }
}
