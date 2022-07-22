using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreGem : MonoBehaviour
{
    [SerializeField] private AudioSource _sound;
    [SerializeField] private int _score;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _destroyDelay = 2;
    [SerializeField] private float _heal = 0;
    private Player _player;
    private GameData _gameData;
    private bool _taken;
    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, GameData gameData)
    {
        _player = player;
        _gameData = gameData;
        gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState obj)
    {
        if (obj == GameState.Restart)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        _gameData.GameStateChanged-= GameStateChanged;
    }
    private void Update()
    {
        if (_taken)
            return;
        if (_player != null /*&& (_heal == 0 || _player.HP.Value<_player.StatsCollection.GetStat(StatType.MaxHP).Value)*/ && (_player.Position - transform.position).magnitude < _player.StatsCollection.GetStat(StatType.PickupRadius).Value)
        {
            _taken = true;
            _sound.Play();
            _animator.SetBool("take", true);
            _player.Gold.AddBaseValue(_score);
            if (_heal > 0)
                _player.ReceiveDamage(new DamageMessage(_player, _player, -_heal, DamageSources.Heal));
            Destroy(gameObject, _destroyDelay);
        }
    }
}
