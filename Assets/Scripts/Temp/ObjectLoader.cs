using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    void Start()
    {
        foreach (var obj in _objects)
        {
            Instantiate(obj, transform);
        }
    }

}
