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

public class ShopShip : MonoBehaviour
{
    [SerializeField] TMP_Text _countdownText;
    [SerializeField] TMP_Text _hintText;
    [SerializeField] Collider2D _collider;
    private AsyncReactiveProperty<int> _countDown = new AsyncReactiveProperty<int>(99);
    private int _countdownValue = 30;
    private Timer _landingTimer;
    private Timer _countdownTimer;
    private Vector3 _newPosition;
    private Vector3 _startPosition;
    private bool _playerInRadius;
    private UIShop _shop;
    private GameData _gameData;
    private CooldownIndicator _shopIndicator;
    private IDisposable _updateDisposible;
    private float _lastArrival;
    private float _cooldownTime = 10;
    private float TimeToArrival => _cooldownTime - (Time.time - _lastArrival);

    [Inject]
    private void Construct(UIShop shop, CooldownsPanel cooldownsPanel, GameData gameData)
    {
        _gameData = gameData;
        _shopIndicator = cooldownsPanel.AddIndiacator(AddressKeys.Ico_Battery);
        _shop = shop;
        _shop.OnClosed += CountDownForceEnd;
        _lastArrival = Time.time;
        _updateDisposible = UniTaskAsyncEnumerable.EveryUpdate().Subscribe(_ => FakeUpdate());
    }

    public void CountDownForceEnd()
    {
        _countdownTimer.ForceEnd();
    }

    private void Awake()
    {
        _countDown.BindTo(_countdownText);
    }
    private void StartLanding()
    {
        _newPosition = (new Vector3(Random.value * 2 - 1, Random.value * 2 - 1).normalized * 25 * Random.value).Get2D();
        _startPosition = _newPosition + Vector3.up * 60 - Vector3.forward * 50;
        _landingTimer = new Timer(2)
            .SetUpdate(x => transform.position = Vector3.Lerp(_startPosition, _newPosition, x))
            .SetEnd(StartCountdown);
    }

    private void StartCountdown()
    {
        _collider.enabled = true;
        _countDown.Value = _countdownValue;
        _countdownText.gameObject.SetActive(true);
        _countdownTimer = new Timer(_countdownValue)
            .SetUpdate(x => _countDown.Value = (int)((1 - x) * _countdownValue))
            .SetEnd(OnCountDownEnd);
    }

    private void OnCountDownEnd()
    {
        _collider.enabled = false;
        _countdownText.gameObject.SetActive(false);
        _landingTimer = new Timer(2)
            .SetUpdate(x => transform.position = Vector3.Lerp(_newPosition, _startPosition, x));
            //.SetEnd(() => gameObject.SetActive(false));
    }

    private void OnDestroy()
    {
        _landingTimer.Stop();
        _countdownTimer.Stop();
        _shop.OnClosed -= CountDownForceEnd;
        _updateDisposible.Dispose();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            _hintText.gameObject.SetActive(true);
            _playerInRadius = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            _hintText.gameObject.SetActive(false);
            _playerInRadius = false;
        }
    }

    private void FakeUpdate()
    {
        if (_gameData.State == GameState.InGame)
        {
            _shopIndicator.SetTime(TimeToArrival, _cooldownTime);
            if (TimeToArrival <= 0)
            {
                _lastArrival = Time.time;
                StartLanding();
            }
        }
    }

    private void Update()
    {
        if (_playerInRadius && Input.GetKeyDown(KeyCode.E))
        {
            _shop.Show();
        }
    }
}
