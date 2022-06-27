using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Umb16.Extensions;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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

    private AsyncReactiveProperty<Player> _player;
    private ComplexStat _rayError;
    private GameData _gameData;
    private CooldownIndicator _colldownIndicator;
    private float? _cashedLenght;
    private bool _twoPointWay;

    private Timer _stopTimer;
    private float _allRayTime;
    private float _rayPathLenght;
    private float _errorLenghtRatio;
    private bool _startDrawIsValid;


    private float CooldownTime => _allRayTime - _rayDelayTime;

    [Inject]
    private async UniTask Construct(AsyncReactiveProperty<Player> player, GameData gameData, UICooldownsManager cooldownsPanel)
    {
        _player = player;
        _player.Where(x => x != null).ForEachAsync(x =>
        {
            _rayError = x.StatsCollection.GetStat(StatType.RayError);
        }).Forget();
        _gameData = gameData;
        _gameData.GameStateChanged += GameStateChanged;
        _colldownIndicator = await cooldownsPanel.AddIndiacator(Addresses.Ico_Laser);
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            cannonPath.Clear();
            rayIsReady = true;
            _stopTimer?.Stop();
            _colldownIndicator.SetTime(0, 1);
            DestroyImmediate(_cannonRay);
        }
    }

    private void Awake()
    {
        _cannonPath = GetComponent<LineRenderer>();
    }
    private void RayLogic()
    {
        if (_gameData == null)
            return;
        if (_gameData.State != GameState.Gameplay)
            return;
        if (Input.GetMouseButtonDown(0) && rayIsReady)
        {
            _startDrawIsValid = true;
        }
            if (Input.GetMouseButton(0) && rayIsReady && _startDrawIsValid)
        {
            if (_cashedLenght == null)
                _cashedLenght = _player.Value.MaxPathLength;
            if (_currentPathLength < _cashedLenght)
            {
                Vector3 pos;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    pos = hit.point;
                }
                else
                    return;
                
                //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition).Get2D();
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
        if (Input.GetMouseButtonUp(0) && rayIsReady && _startDrawIsValid)
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
            _errorLenghtRatio = 1;
            if (_rayError.Value > 0)
            {
                cannonPath = cannonPath.Select(x => x + new Vector3(Random.value, Random.value).normalized * Random.value * _rayError.Value).ToList();
                _errorLenghtRatio = LenghtOfPath(cannonPath) / _rayPathLenght;
            }

        }
        if (cannonPath.Count <= 1 || _currentLineIndex != 0)
        {
            return;
        }
        _rayDelayTime += Time.deltaTime;
        _allRayTime = _rayPathLenght / _player.Value.RaySpeed + _player.Value.RayDelay;
        _colldownIndicator.SetTime(CooldownTime, _allRayTime);
        if (_rayDelayTime > _player.Value.RayDelay)
        {
            if (_cannonRay == null)
            {
                //_cannonPath.positionCount = 0;
                _cannonRay = Instantiate(_cannonRayPrefab);
                _cannonRay.GetComponent<RayScript>().SetSplash(_player.Value.RaySplash);
            }
            rayTime += Time.deltaTime * _player.Value.RaySpeed * _errorLenghtRatio;

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
        _startDrawIsValid = false;
        SoundManager.Instance.PlayRayReady();
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
