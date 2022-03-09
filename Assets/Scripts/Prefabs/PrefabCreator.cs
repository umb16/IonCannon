using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class PrefabCreator
{
    public static async UniTask<T> Get<T>(string name)
    {
        return await Addressables.LoadAssetAsync<T>(name).Task;
    }
}
