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
    private Player _player;
    private bool _taken;
    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;
    }

    private void Update()
    {
        if (_taken)
            return;
        if (_player != null && (_player.Position - transform.position).magnitude < _player.StatsCollection.GetStat(StatType.PickupRadius).Value)
        {
            _taken = true;
            _sound.Play();
            _animator.SetBool("take", true);
            _player.Gold.AddBaseValue(_score);
            //_player.ReceiveDamage(new DamageMessage(null, _player, -.2f, DamageSources.Heal));
            Destroy(gameObject, 2);
        }
    }
}
