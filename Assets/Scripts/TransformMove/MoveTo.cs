using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private Vector3 _velocity;

    private void Update()
    {
        transform.position+=_velocity * Time.deltaTime;
    }
}
