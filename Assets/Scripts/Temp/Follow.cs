using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _shift;
    [SerializeField] private Vector2 _mapSize = new Vector2(100, 100);
    [SerializeField] private float _cameraToCursor;
    [SerializeField] private float _smoothFactor = 10;

    private Vector2 _cameraSize;
    private AsyncReactiveProperty<Player> _player;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;
    }

    private void Start()
    {
        float ortSize = Camera.main.orthographicSize;
        _cameraSize.y = ortSize;
        _cameraSize.x = ortSize * Camera.main.aspect;
    }

    void Update()
    {
        if (_player?.Value != null)
        {
            _target = _player.Value.transform;
            Vector2 pos;
            if (_cameraToCursor != 0)
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos = Vector3.LerpUnclamped(_target.position, pos, _cameraToCursor);
            }
            else
                pos = _target.position;
            transform.position = CheckCamera(Vector3.Lerp(transform.position, (Vector3)pos + _shift, Time.deltaTime * _smoothFactor));
        }
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
