using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InstantiateFromAnimator : MonoBehaviour
{
    [SerializeField] private AssetReference _particle;
    [SerializeField] private Transform[] _points;

    public void Instantiate(int index)
    {
        _particle.InstantiateAsync(_points[index].position, Quaternion.identity);  
    }
}
