using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RayDrawer : MonoBehaviour
{
    [SerializeField] private GameObject _cannonRayPrefab;
    [SerializeField] private LayerMask _rayTaregetMask;

    private int _currentLineIndex;

    private float _currentPathLength;

    private List<Vector2> cannonPath = new List<Vector2>();

    private Vector3 _oldPointOfPath;

    private GameObject _cannonRay;

    private float rayTime;

    private float _rayDelayTime;

    private bool rayIsReady = true;

    private LineRenderer _cannonPath;

    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        _cannonPath = GetComponent<LineRenderer>();
    }
    private void RayLogic()
    {
        if (Input.GetMouseButton(0) && _currentPathLength < _player.MaxPathLength && rayIsReady)
        {
            /*if (SetPerc != null)
            {
                SetPerc();
                SetPerc = null;
            }*/
            Vector3 zero = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition * Camera.main.rect.width);
            Debug.DrawRay(ray.origin, ray.direction);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _rayTaregetMask.value))
            {
                return;
            }
            zero = hitInfo.point;
            zero.z = zero.y * .1f;
            _cannonPath.positionCount = _currentLineIndex + 1;
            if (_currentLineIndex > 0)
            {
                if (_player.MaxPathLength > _currentPathLength + Vector3.Distance(zero, _oldPointOfPath))
                {
                    _currentPathLength += Vector3.Distance(zero, _oldPointOfPath);
                }
                else
                {
                    float d = _player.MaxPathLength - _currentPathLength;
                    zero = _oldPointOfPath + (zero - _oldPointOfPath).normalized * d;
                    _currentPathLength = _player.MaxPathLength;
                }
            }
            else
            {
                cannonPath.Clear();
            }
            cannonPath.Add(zero);
            _cannonPath.SetPosition(_currentLineIndex, zero);
            _oldPointOfPath = zero;
            _currentLineIndex++;
        }
        if (Input.GetMouseButtonUp(0) && rayIsReady)
        {
            rayIsReady = false;
            _currentLineIndex = 0;
            _currentPathLength = 0f;
            rayTime = 0f;
            _rayDelayTime = 0f;
            _cannonPath.positionCount = 0;
        }
        if (cannonPath.Count <= 1 || _currentLineIndex != 0)
        {
            return;
        }
        _rayDelayTime += Time.deltaTime;
        if (_rayDelayTime > _player.RayDelay)
        {
            if (_cannonRay == null)
            {
                _cannonRay = Instantiate(_cannonRayPrefab);
                _cannonRay.GetComponent<RayScript>().SetSplash(_player.RaySplash);
            }
            rayTime += Time.deltaTime * _player.RaySpeed;
            _cannonRay.transform.position = Vector3.Lerp(cannonPath[0], cannonPath[1], rayTime / Vector2.Distance(cannonPath[0], cannonPath[1]));
            if (rayTime > Vector2.Distance(cannonPath[0], cannonPath[1]))
            {
                rayTime -= Vector2.Distance(cannonPath[0], cannonPath[1]);
                cannonPath.RemoveAt(0);
            }
            if (cannonPath.Count < 2)
            {
                _cannonRay.GetComponent<RayScript>().Stop();
                _cannonRay = null;
                rayIsReady = true;
            }
        }
    }

    private void Update()
    {
        RayLogic();
    }
}
