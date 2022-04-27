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
        if ((_player.Position - transform.position).magnitude < 2)
        {
            _player.Gold.AddBaseValue(_score);
            Destroy(gameObject);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D col)
    {
        var mob = col.gameObject.GetComponent<IMob>();
        if (mob != null && mob == (IMob)_player)
        {
            //_player.Exp.AddExp(_score);
            _player.Gold.AddBaseValue(_score);
            Destroy(gameObject);
        }
    }*/
}
