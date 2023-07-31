using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
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
    private float? _cashedLength;
    private bool _twoPointWay;

    private Timer _stopTimer;
    private float _allRayTime;
    private float _rayPathLength;
    private float _errorLengthRatio;
    private bool _startDrawIsValid;
    private float _rayCostCorrectionValue;
    private RayCollider _rayCollider;

    public float LengthLeft
    {
        get
        {
            return _player.Value.Energy;
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
        _gameData.OnReset += OnGameReset;
    }

    private void OnGameReset()
    {
        cannonPath.Clear();
        rayIsReady = true;
        _stopTimer?.Stop();
        DestroyImmediate(_cannonRay);
        _fakeCursor.SetWait(CursorType.Normal);
    }

    private void Awake()
    {
        _cannonPath = GetComponent<LineRenderer>();
    }
    private void RayLogic()
    {
        if (_gameData == null)
            return;
        if (_gameData.Status != GameState.Gameplay)
            return;
        if (Input.GetMouseButtonDown(0) && rayIsReady)
        {
            _startDrawIsValid = true;
            _oldPathLength = 0;
            _player.Value.AddEnergy(-20);
            _player.Value.EnergyRegen(false);
        }
        if (Input.GetMouseButton(0) && rayIsReady && _startDrawIsValid)
        {
            if (_cashedLength == null)
                _cashedLength = _player.Value.Energy;
            if (LengthLeft > 0)
            {
                Vector3 newPos;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100000, _rayTaregetMask.value))
                {
                    newPos = hit.point;
                }
                else
                    return;

                if (newPos.EqualsWithThreshold(_oldPointOfPath, 0.1f))
                    return;
                _cannonPath.positionCount = _currentLineIndex + 1;
                if (_currentLineIndex > 0)
                {
                    var distanceFromOldPoint = Vector3.Distance(newPos, _oldPointOfPath);
                    if (LengthLeft > distanceFromOldPoint)
                    {
                        _currentPathLength += distanceFromOldPoint;
                    }
                    else
                    {
                        var validDistance = LengthLeft;
                        newPos = _oldPointOfPath + (newPos - _oldPointOfPath).normalized * validDistance;
                        _currentPathLength += validDistance;
                    }
                    var rayEnergyCost = (_oldPathLength - _currentPathLength) * (1 - RayCost—orrection());
                    _player.Value.AddEnergy(rayEnergyCost);
                    _oldPathLength = _currentPathLength;
                }
                else
                {
                    cannonPath.Clear();
                }
                cannonPath.Add(new RayPathPoint() { point = newPos, energyRatio = _player.Value.Energy / _player.Value.Capacity });
                _cannonPath.SetPosition(_currentLineIndex, newPos);
                _oldPointOfPath = newPos;
                _currentLineIndex++;
                _fakeCursor.SetWait(CursorType.Draw);
            }
        }
        if (Input.GetMouseButtonUp(0) && rayIsReady && _startDrawIsValid)
        {
            if (cannonPath.Count != 0)
            {
                if (cannonPath.Count == 1)
                {
                    cannonPath.Add(cannonPath[0]);
                    _player.Value.AddEnergy(-20);
                }
                if (_currentPathLength < 1f || cannonPath.Count == 2)
                    _twoPointWay = true;
                else
                    _twoPointWay = false;
                rayIsReady = false;
                _fakeCursor.SetWait(CursorType.Wait);
                _player.Value.EnergyRegen(true);
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
            _cashedLength = null;
            _cannonPath.positionCount = 0;
            _rayPathLength = LenghtOfPath(cannonPath);
            _errorLengthRatio = 1;
            if (_rayError.Value > 0)
            {
                cannonPath.ForEach(x => x.point += new Vector3(Random.value, Random.value).normalized * Random.value * _rayError.Value);
                _errorLengthRatio = LenghtOfPath(cannonPath) / _rayPathLength;
            }

        }
        if (cannonPath.Count <= 1 || _currentLineIndex != 0)
        {
            return;
        }
        _rayDelayTime += Time.deltaTime;
        _allRayTime = _rayPathLength / _player.Value.RaySpeed + _player.Value.RayDelay;
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
            rayTime += Time.deltaTime * _player.Value.RaySpeed * _errorLengthRatio;

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
    public float RayCost—orrection()
    {
        return 0;
        _rayCostCorrectionValue = 0;

        if (_player.Value.RayCostReduction == 0)
        {
            _rayCostCorrectionValue = (_currentPathLength / (50/*/_player.Value.RayCostReduction*/));

            if (_rayCostCorrectionValue > .9f)
                _rayCostCorrectionValue = .9f;
            return _rayCostCorrectionValue;
        }

        return _rayCostCorrectionValue;
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
