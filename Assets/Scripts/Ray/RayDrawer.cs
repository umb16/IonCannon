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

public class RayPathPoint
{
    public Vector3 point;
    public float energyRatio;
}

public class RayDrawer : MonoBehaviour
{
    [SerializeField] private GameObject _cannonRayPrefab;
    [SerializeField] private LayerMask _rayTaregetMask;

    private int _currentLineIndex;

    private float _currentPathLength;
    private float _oldPathLength;

    private List<RayPathPoint> cannonPath = new List<RayPathPoint>();

    private Vector3 _oldPointOfPath;

    private GameObject _cannonRay;

    private float rayTime;

    private float _rayDelayTime;

    private bool rayIsReady = true;

    private LineRenderer _cannonPath;

    private AsyncReactiveProperty<Player> _player;
    private FakeCursor _fakeCursor;
    private ComplexStat _rayError;
    private GameData _gameData;
    private float? _cashedLenght;
    private bool _twoPointWay;

    private Timer _stopTimer;
    private float _allRayTime;
    private float _rayPathLenght;
    private float _errorLenghtRatio;
    private bool _startDrawIsValid;
    private RayCollider _rayCollider;

    public float MaxLenght
    {
        get
        {
            if (_cashedLenght == null)
                return _player.Value.Energy;
            else
                return Mathf.Max(_cashedLenght.Value, _player.Value.Energy);
        }
    }


    private float CooldownTime => _allRayTime - _rayDelayTime;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, GameData gameData, UICooldownsManager cooldownsPanel, FakeCursor fakeCursor)
    {
        _player = player;
        _fakeCursor = fakeCursor;
        _player.Where(x => x != null).ForEachAsync(x =>
        {
            _rayError = x.StatsCollection.GetStat(StatType.RayError);
        }).Forget();
        _gameData = gameData;
        _gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            cannonPath.Clear();
            rayIsReady = true;
            _stopTimer?.Stop();
            DestroyImmediate(_cannonRay);
            _fakeCursor.SetWait(CursorType.Normal);
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
            _oldPathLength = -20;
            //_player.Value.AddEnergy(-20);
            _player.Value.EnergyRegen(false);
        }
        if (Input.GetMouseButton(0) && rayIsReady && _startDrawIsValid)
        {
            if (_cashedLenght == null)
                _cashedLenght = _player.Value.Energy;
            if (_currentPathLength < MaxLenght)
            {
                Vector3 pos;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100000, _rayTaregetMask.value))
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
                    if (MaxLenght > _currentPathLength + Vector3.Distance(pos, _oldPointOfPath))
                    {
                        _currentPathLength += Vector3.Distance(pos, _oldPointOfPath);
                    }
                    else
                    {
                        float d = (float)MaxLenght - _currentPathLength;
                        pos = _oldPointOfPath + (pos - _oldPointOfPath).normalized * d;
                        _currentPathLength = (float)MaxLenght;
                    }
                    _player.Value.AddEnergy(_oldPathLength - _currentPathLength);
                    _oldPathLength = _currentPathLength;
                }
                else
                {
                    cannonPath.Clear();
                }
                cannonPath.Add(new RayPathPoint() { point = pos, energyRatio = _player.Value.Energy / _player.Value.Capacity });
                _cannonPath.SetPosition(_currentLineIndex, pos);
                _oldPointOfPath = pos;
                _currentLineIndex++;
                _fakeCursor.SetWait(CursorType.Draw);
            }
        }
        if (Input.GetMouseButtonUp(0) && rayIsReady && _startDrawIsValid)
        {
            if (cannonPath.Count != 0)
            {
                if (cannonPath.Count == 1)
                    cannonPath.Add(cannonPath[0]);
                if (_currentPathLength < .2f || cannonPath.Count == 2)
                    _twoPointWay = true;
                else
                    _twoPointWay = false;
                rayIsReady = false;
                _fakeCursor.SetWait(CursorType.Wait);
                _player.Value.EnergyRegen(true);
                //_player.Value.EnergyRegen(false);
            }
            if (_player.Value.RayReverse.Value > 0)
            {
                var cannonPathTemp = cannonPath.ToList();
                for (int i = 0; i < _player.Value.RayReverse.Value; i++)
                {
                    if (i % 2 == 0)
                    {
                        cannonPath.AddRange(cannonPathTemp.Reverse<RayPathPoint>().ToList());
                    }
                    else
                    {
                        cannonPath.AddRange(cannonPathTemp);
                    }
                }

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
                cannonPath.ForEach(x => x.point += new Vector3(Random.value, Random.value).normalized * Random.value * _rayError.Value);
                _errorLenghtRatio = LenghtOfPath(cannonPath) / _rayPathLenght;
            }

        }
        if (cannonPath.Count <= 1 || _currentLineIndex != 0)
        {
            return;
        }
        _rayDelayTime += Time.deltaTime;
        _allRayTime = _rayPathLenght / _player.Value.RaySpeed + _player.Value.RayDelay;
        if (_rayDelayTime > _player.Value.RayDelay)
        {
            if (_cannonRay == null)
            {
                //_cannonPath.positionCount = 0;
                _cannonRay = Instantiate(_cannonRayPrefab);
                _cannonRay.GetComponent<RayScript>().SetSplash(_player.Value.RaySplash);
                _rayCollider = _cannonRay.GetComponentInChildren<RayCollider>();
                if (cannonPath[0].energyRatio < .3f)
                    _rayCollider.DamageMultiplier = Mathf.Max(.3f, cannonPath[0].energyRatio * 3);
            }
            rayTime += Time.deltaTime * _player.Value.RaySpeed * _errorLenghtRatio;

            _cannonRay.transform.position = Vector3.Lerp(cannonPath[0].point, cannonPath[1].point, rayTime / Vector2.Distance(cannonPath[0].point, cannonPath[1].point));
            if (rayTime > Vector2.Distance(cannonPath[0].point, cannonPath[1].point))
            {
                rayTime -= Vector2.Distance(cannonPath[0].point, cannonPath[1].point);
                if (cannonPath[0].energyRatio < .3f)
                    _rayCollider.DamageMultiplier = Mathf.Max(.3f, cannonPath[0].energyRatio * 3);
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

    private float LenghtOfPath(List<RayPathPoint> vectors)
    {
        return vectors.Zip(vectors.Skip(1), (a, b) => Vector2.Distance(a.point, b.point)).Sum();
    }

    private void StopRay()
    {
        _cannonRay?.GetComponent<RayScript>()?.Stop();
        _cannonRay = null;
        rayIsReady = true;
        _startDrawIsValid = false;
        SoundManager.Instance.PlayRayReady();
        _fakeCursor.SetWait(CursorType.Normal);
        // _player.Value.EnergyRegen(true);
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
