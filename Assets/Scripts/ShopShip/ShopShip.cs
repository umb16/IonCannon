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

public class ShopShip : MonoBehaviour
{
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject _forceField;
    [SerializeField] private StandartZoneIndicator _zoneIndiacator;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _dust;

    private AsyncReactiveProperty<int> _countDown = new AsyncReactiveProperty<int>(99);
    private int _countdownValue = 30;
    private Timer _landingTimer;
    private Timer _countdownTimer;
    private Vector3 _newPosition;
    private Vector3 _startPosition;
    private UIShopLayer _shop;
    private GameData _gameData;
    private CooldownIndicator _shopIndicator;
    private float _lastArrival;
    private AsyncReactiveProperty<Player> _player;
    private float _cooldownTime = 60;
    private float TimeToArrival => _cooldownTime - (Time.time - _lastArrival);
    private bool _targetZoneSetted = false;

    [Inject]
    private async UniTask Construct(UICooldownsManager cooldownsManager, GameData gameData, AsyncReactiveProperty<Player> player)
    {
        _gameData = gameData;
        _gameData.GameStarted += OnGameStarted;
        _shop = BaseLayer.ForceGet<UIShopLayer>();
        _shop.OnClosed += CountDownForceEnd;
        _lastArrival = Time.time;
        _player = player;
        _shopIndicator = await cooldownsManager.AddIndiacator(AddressKeys.Ico_Ship);
        gameData.GameStateChanged += GameStateChanged;

    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            _startPosition = _newPosition + Vector3.up * 100 - Vector3.forward * 50;
            OnCountDownEnd();
            _landingTimer.ForceEnd();
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
        _collider.enabled = true;
        _countDown.Value = _countdownValue;
        _countdownText.gameObject.SetActive(true);
        _forceField.SetActive(true);
        _countdownTimer = new Timer(_countdownValue)
            .SetUpdate(x => _countDown.Value = (int)((1 - x) * _countdownValue))
            .SetEnd(OnCountDownEnd);
    }

    private void OnCountDownEnd()
    {
        _dust.SetActive(false);
        _forceField.SetActive(false);
        _collider.enabled = false;
        _targetZoneSetted = false;
        _zoneIndiacator.gameObject.SetActive(false);
        //_forceField.transform.localScale = Vector3.one * 3;
        _countdownText.gameObject.SetActive(false);
        _animator.SetBool("Idle", false);
        _landingTimer = new Timer(2)
            .SetUpdate(x =>
            {
                transform.position = Vector3.Lerp(_newPosition, _startPosition, x);
            });
        //.SetEnd(() => gameObject.SetActive(false));
    }

    private void OnDestroy()
    {
        _landingTimer?.Stop();
        _countdownTimer?.Stop();
        _shop.OnClosed -= CountDownForceEnd;
        _gameData.GameStarted -= OnGameStarted;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            //_hintText.gameObject.SetActive(true);
            BaseLayer.Show<UIShopLayer>();
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
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
            if (TimeToArrival <= 5 && _targetZoneSetted == false)
            {
                _zoneIndiacator.SetBlink(.5f);
                _targetZoneSetted = true;
                _newPosition = _newPosition = (_player.Value.Position + (new Vector3(Random.value * 2 - 1, Random.value * 2 - 1).normalized * 5 * Random.value)).Get2D();
                //Vector3 indicatorPosition = _newPosition;
                //indicatorPosition.z = 1;
                _zoneIndiacator.gameObject.SetActive(true);
                _zoneIndiacator.SetPosition(_newPosition);

            }
            if (TimeToArrival <= 0)
            {
                _lastArrival = Time.time;
                StartLanding();
            }
        }
    }
}
