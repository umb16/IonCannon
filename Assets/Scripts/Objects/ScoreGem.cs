using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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
        gameData.OnReset += OnGameReset;
    }

    private void OnGameReset()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _gameData.OnReset -= OnGameReset;
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
            _player.ScoreGemPickingUp(_score, _heal);                 
            Destroy(gameObject, _destroyDelay);
        }
    }
}
