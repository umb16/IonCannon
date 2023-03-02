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
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private Collider _collider;
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
    private AsyncReactiveProperty<Player> _player;
    private LifeSupportTower _lifeSupportTower;
    private float _cooldownTime = 60;
    private float TimeToArrival => _cooldownTime - (Time.time - _lastArrival);
    private bool _targetZoneSetted = false;
    private bool _targetZoneShearchStart = false;

    public bool Landed => _collider.enabled;

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
        gameData.GameStateChanged += GameStateChanged;

    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            _countdownTimer?.ForceEnd();
            _landingTimer?.ForceEnd();
            DisableLandingState();
            transform.position = _newPosition + Vector3.up * 100 - Vector3.forward * 50;          
        }
    }

    public void SetLastArrival(float shift)
    {
        _lastArrival = Time.time - (_cooldownTime - shift);
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
        _zoneIndiacator.SetRadius(5);
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

        _interactionTimer = new Timer(.1f).SetEnd(()=> _collider.enabled = true);
        _countdownTimer = new Timer(_countdownValue)
            .SetUpdate(x => _countDown.Value = (int)((1 - x) * _countdownValue))
            .SetEnd(OnCountDownEnd);
    }

    private void OnCountDownEnd()
    {
        DisableLandingState();
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
        _collider.enabled = false;
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
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            _gameData.UIStatus = UIStates.Shop;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //_hintText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_gameData.State == GameState.Gameplay)
        {
            _shopIndicator.SetTime(TimeToArrival, _cooldownTime);
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
                    specialPos = Vector3.Lerp(_zoneIndiacator.transform.position, ((Vector3)_lifeSupportTower.GetNerestPoint(_player.Value.Position)).Get2D(), Time.deltaTime*2);
                }
                _zoneIndiacator.SetPosition(specialPos);
                if (TimeToArrival <= 5 && _targetZoneSetted == false)
                {
                    _zoneIndiacator.SetRadius(5);
                    _zoneIndiacator.SetBlink(.5f);
                    _targetZoneSetted = true;
                    //_newPosition = _newPosition = (_player.Value.Position + (new Vector3(Random.value * 2 - 1, Random.value * 2 - 1).normalized * 5 * Random.value)).Get2D();
                    _newPosition = specialPos;
                    //_zoneIndiacator.gameObject.SetActive(true);
                    _zoneIndiacator.SetPosition(_newPosition);

                }
            }
            
            if (TimeToArrival <= 0)
            {
                _lastArrival = Time.time;
                StartLanding();
            }
        }
    }
}
