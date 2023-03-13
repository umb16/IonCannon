using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using UnityEngine.Localization;
using SPVD.LifeSupport;

public class ShopShip : MonoBehaviour
{
    private static float COOLDOWN_TIME = 60;

    [SerializeField] private float _zoneRadius;
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private GameObject _forceField;
    [SerializeField] private StandartZoneIndicator _zoneIndiacator;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _dust;

    private AsyncReactiveProperty<int> _countDown = new AsyncReactiveProperty<int>(99);
    private int _countdownValue = 30;
    private Timer _landingTimer;
    private Timer _countdownTimer;
    private Timer _interactionTimer;
    private Vector3 _newPosition;
    private Vector3 _startPosition;
    private UIShopLayer _shop;
    private GameData _gameData;
    private CooldownIndicator _shopIndicator;
    private float _lastArrival;
    private bool _startLanding;
    private AsyncReactiveProperty<Player> _player;
    private LifeSupportTower _lifeSupportTower;
    private float TimeToArrival => COOLDOWN_TIME - (Time.time - _lastArrival);
    private bool _targetZoneSetted = false;
    private bool _targetZoneShearchStart = false;
    private bool _shopOpen;
    public bool Landed { get; private set; }

    [Inject]
    private async UniTask Construct(UICooldownsManager cooldownsManager, GameData gameData,
        AsyncReactiveProperty<Player> player, LifeSupportTower lifeSupportTower, UIShopLayer uiShop)
    {
        _gameData = gameData;
        _gameData.GameStarted += OnGameStarted;
        _shop = uiShop;
        _shop.OnClosed += CountDownForceEnd;
        _lastArrival = Time.time;
        _player = player;
        _lifeSupportTower = lifeSupportTower;
        _shopIndicator = await cooldownsManager.AddIndiacator(Addresses.Ico_Ship);
        gameData.OnReset += OnGameReset;

    }

    private void OnGameReset()
    {
        _countdownTimer?.ForceEnd();
        _landingTimer?.ForceEnd();
        DisableLandingState();
        transform.position = _newPosition + Vector3.up * 100 - Vector3.forward * 50;
    }

    public void SetLastArrival(float shift)
    {
        _lastArrival = Time.time - (COOLDOWN_TIME - shift);
    }

    public void CountDownForceEnd()
    {
        _countdownTimer?.ForceEnd();
    }

    public void OnGameStarted()
    {
        _lastArrival = Time.time;
    }

    private void Awake()
    {
        _countDown.BindTo(_countdownText);
        _zoneIndiacator.SetRadius(_zoneRadius);
    }
    private void StartLanding()
    {
        _startPosition = _newPosition + Vector3.up * 100 - Vector3.forward * 50;
        _landingTimer = new Timer(2)
            .SetUpdate(x =>
            {
                //1.0f - (1.0f - x) * (1.0f - x);
                float factor = .7f;
                var y = (1.0f - Mathf.Pow((1.0f - x), 2 * factor));
                transform.position = Vector3.Lerp(_startPosition, _newPosition, y);
            })
            .SetEnd(StartCountdown);
    }

    private void StartCountdown()
    {
        _animator.SetBool("Idle", true);
        _dust.SetActive(true);
        _zoneIndiacator.SetBlink(0);

        _countDown.Value = _countdownValue;
        _countdownText.gameObject.SetActive(true);
        _forceField.SetActive(true);

        _interactionTimer = new Timer(.1f).SetEnd(() => Landed = true);
        _countdownTimer = new Timer(_countdownValue)
            .SetUpdate(x => _countDown.Value = (int)((1 - x) * _countdownValue))
            .SetEnd(OnCountDownEnd);
    }

    private void OnCountDownEnd()
    {
        DisableLandingState();
        _startLanding = false;
        _lastArrival = Time.time;
        _landingTimer = new Timer(2)
            .SetUpdate(x =>
            {
                transform.position = Vector3.Lerp(_newPosition, _startPosition, x);
            });
    }

    private void DisableLandingState()
    {
        _dust.SetActive(false);
        _forceField.SetActive(false);
        _interactionTimer?.Stop();
        Landed = false;
        _shopOpen = false;
        _targetZoneSetted = false;
        _targetZoneShearchStart = false;
        _zoneIndiacator.gameObject.SetActive(false);
        _countdownText.gameObject.SetActive(false);
        _animator.SetBool("Idle", false);
    }

    private void OnDestroy()
    {
        _landingTimer?.Stop();
        _countdownTimer?.Stop();
        _shop.OnClosed -= CountDownForceEnd;
        _gameData.GameStarted -= OnGameStarted;
    }

    private void Update()
    {
        if (_gameData.Status == GameState.Gameplay)
        {
            if (Landed && !_shopOpen)
            {
                if (Vector3.Distance(_player.Value.Position, _zoneIndiacator.transform.position) < _zoneRadius)
                {
                    _gameData.SetState(GameState.InShop);
                    _shopOpen = true;
                }
            }

            _shopIndicator.SetTime(Mathf.Max(0, TimeToArrival), COOLDOWN_TIME);
            if (_startLanding)
                return;
            if (TimeToArrival <= 10 && !_targetZoneSetted)
            {
                Vector3 specialPos;
                if (!_targetZoneShearchStart)
                {
                    _zoneIndiacator.SetRadius(2);
                    _zoneIndiacator.gameObject.SetActive(true);
                    _targetZoneShearchStart = true;
                    specialPos = ((Vector3)_lifeSupportTower.GetNerestPoint(_player.Value.Position)).Get2D();

                }
                else
                {
                    specialPos = Vector3.Lerp(_zoneIndiacator.transform.position, ((Vector3)_lifeSupportTower.GetNerestPoint(_player.Value.Position)).Get2D(), Time.deltaTime * 2);
                }
                _zoneIndiacator.SetPosition(specialPos);
                if (TimeToArrival <= 5 && _targetZoneSetted == false)
                {
                    _zoneIndiacator.SetRadius(_zoneRadius);
                    _zoneIndiacator.SetBlink(.5f);
                    _targetZoneSetted = true;
                    _newPosition = specialPos;
                    _zoneIndiacator.SetPosition(_newPosition);

                }
            }

            if (TimeToArrival <= 2)
            {
                _startLanding = true;
                StartLanding();
            }
        }
    }
}
