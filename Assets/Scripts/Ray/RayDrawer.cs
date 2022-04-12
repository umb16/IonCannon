using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Umb16.Extensions;
using UnityEngine;
using Zenject;

public class RayDrawer : MonoBehaviour
{
    [SerializeField] private GameObject _cannonRayPrefab;
    [SerializeField] private LayerMask _rayTaregetMask;

    private int _currentLineIndex;

    private float _currentPathLength;

    private List<Vector3> cannonPath = new List<Vector3>();

    private Vector3 _oldPointOfPath;

    private GameObject _cannonRay;

    private float rayTime;

    private float _rayDelayTime;

    private bool rayIsReady = true;

    private LineRenderer _cannonPath;

    private Player _player;
    private GameData _gameData;
    private CooldownIndicator _colldownIndicator;
    private float? _cashedLenght;
    private bool _twoPointWay;

    private Timer _stopTimer;
    private float _allRayTime;
    private float _rayPathLenght;

    private float CooldownTime => _allRayTime - _rayDelayTime;

    [Inject]
    private void Construct(Player player, GameData gameData, CooldownsPanel cooldownsPanel)
    {
        _player = player;
        _gameData = gameData;
        _colldownIndicator = cooldownsPanel.AddIndiacator(AddressKeys.Ico_Laser);
    }

    private void Awake()
    {
        _cannonPath = GetComponent<LineRenderer>();
    }
    private void RayLogic()
    {
        if (_gameData.State != GameState.InGame)
            return;
        if (Input.GetMouseButton(0) && rayIsReady)
        {
            if (_cashedLenght == null)
                _cashedLenght = _player.MaxPathLength;
            if (_currentPathLength < _cashedLenght)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition).Get2D();
                if (pos.EqualsWithThreshold(_oldPointOfPath, 0.01f))
                    return;
                _cannonPath.positionCount = _currentLineIndex + 1;
                if (_currentLineIndex > 0)
                {
                    if (_cashedLenght > _currentPathLength + Vector3.Distance(pos, _oldPointOfPath))
                    {
                        _currentPathLength += Vector3.Distance(pos, _oldPointOfPath);
                    }
                    else
                    {
                        float d = (float)_cashedLenght - _currentPathLength;
                        pos = _oldPointOfPath + (pos - _oldPointOfPath).normalized * d;
                        _currentPathLength = (float)_cashedLenght;
                    }
                }
                else
                {
                    cannonPath.Clear();
                }
                cannonPath.Add(pos);
                _cannonPath.SetPosition(_currentLineIndex, pos);
                _oldPointOfPath = pos;
                _currentLineIndex++;
            }
        }
        if (Input.GetMouseButtonUp(0) && rayIsReady)
        {
            if (cannonPath.Count != 0)
            {
                if (cannonPath.Count == 1)
                    cannonPath.Add(cannonPath[0]);
                _twoPointWay = cannonPath.Count == 2;
                rayIsReady = false;
            }

            _currentLineIndex = 0;
            _currentPathLength = 0f;
            rayTime = 0f;
            _rayDelayTime = 0f;
            _cashedLenght = null;
            _cannonPath.positionCount = 0;

            _rayPathLenght = LenghtOfPath(cannonPath);
        }
        if (cannonPath.Count <= 1 || _currentLineIndex != 0)
        {
            return;
        }
        _rayDelayTime += Time.deltaTime;
        _allRayTime = _rayPathLenght / _player.RaySpeed + _player.RayDelay;
        _colldownIndicator.SetTime(CooldownTime, _allRayTime);
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
                _stopTimer?.ForceEnd();
                _stopTimer = null;
                if (_twoPointWay)
                    _stopTimer = new Timer(1).SetEnd(StopRay);
                else
                    StopRay();
            }
        }
    }

    private float LenghtOfPath(List<Vector3> vectors)
    {
        return vectors.Zip(vectors.Skip(1), (a, b) => Vector2.Distance(a, b)).Sum();
    }

    private void StopRay()
    {
        _cannonRay.GetComponent<RayScript>().Stop();
        _cannonRay = null;
        rayIsReady = true;
    }

    private void OnDestroy()
    {
        _stopTimer?.ForceEnd();
    }

    private void Update()
    {
        RayLogic();
    }
}
