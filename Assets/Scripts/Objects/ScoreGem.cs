using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreGem : MonoBehaviour
{
    [SerializeField] private int _score;
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Update()
    {
        if (_player != null && (_player.Position - transform.position).magnitude < _player.StatsCollection.GetStat(StatType.PickupRadius).Value)
        {
            _player.Gold.AddBaseValue(_score);
            Destroy(gameObject);
        }
    }
}
