using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _shift;
    [SerializeField] private Vector2 _mapSize = new Vector2(100, 100);
    private Vector2 _cameraSize;

    private void Start()
    {
        float ortSize = Camera.main.orthographicSize;
        _cameraSize.y = ortSize;
        _cameraSize.x = ortSize * Camera.main.aspect;

    }

    void Update()
    {
        if (_target != null)
            transform.position = CheckCamera(Vector3.Lerp(transform.position, _target.position + _shift, Time.deltaTime * 10));
    }

    private Vector3 CheckCamera(Vector3 pos)
    {
        if (pos.y - _cameraSize.y < -_mapSize.y / 2)
            pos.y = -_mapSize.y / 2 + _cameraSize.y;
        if (pos.y + _cameraSize.y > _mapSize.y / 2)
            pos.y = _mapSize.y / 2 - _cameraSize.y;
        if (pos.x - _cameraSize.x < -_mapSize.x / 2)
            pos.x = -_mapSize.x / 2 + _cameraSize.x;
        if (pos.x + _cameraSize.x > _mapSize.x / 2)
            pos.x = _mapSize.x / 2 - _cameraSize.x;
        return pos;
    }
}
