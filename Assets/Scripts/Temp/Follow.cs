using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _shift;

    void Update()
    {
        if (_target != null)
            transform.position = Vector3.Lerp(transform.position, _target.position + _shift, Time.deltaTime * 10);
    }
}
