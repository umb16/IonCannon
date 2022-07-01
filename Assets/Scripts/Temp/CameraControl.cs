using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Camera _camera2;
    [SerializeField] private float _cameraToCursor;
    [SerializeField] private float _smoothFactor = 10;
    [SerializeField][Range(1, 990)] private float _distance = 100;
    [SerializeField][Range(2, 120)] private float _fov = 14;
    [SerializeField][Range(1, 80)] private float _angle = 42;
    Transform _target;

    private AsyncReactiveProperty<Player> _player;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;
    }

    private void Start()
    {
        UniTaskAsyncEnumerable.EveryValueChanged(this, x => x._distance).Subscribe(x => _camera.transform.localPosition = new Vector3(0, 0, -x));
        UniTaskAsyncEnumerable.EveryValueChanged(this, x => x._fov).Subscribe(x =>
        { 
            _camera.fieldOfView = _fov;
            _camera2.fieldOfView = _fov;
        });
        UniTaskAsyncEnumerable.EveryValueChanged(this, x => x._angle).Subscribe(x => transform.eulerAngles = new Vector3(-x, 0, 0));

    }

    void Update()
    {
        if (_player?.Value != null)
        {
            _target = _player.Value.transform;
            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _smoothFactor);
        }
    }
}
