using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour
{
    [SerializeField] private float _destroyDelay;
    void Start()
    {
        Destroy(gameObject, _destroyDelay);
    }

}
