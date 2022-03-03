using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimple : Mob
{
    [SerializeField] private float _hp = 1;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _score = 1;
    [SerializeField] private float _size = 1;
    private SpriteRenderer _spriteRenderer;
    private Timer _timer;
    private void Awake()
    {
        StatsCollection = StatsCollectionsDB.StandartEnemy();
        StatsCollection.SetStat(StatType.MaxHP, _hp);
        StatsCollection.SetStat(StatType.HP, _hp);
        StatsCollection.SetStat(StatType.MovementSpeed, _speed);
        StatsCollection.SetStat(StatType.Score, _score);
        StatsCollection.SetStat(StatType.Size, _size);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        DamageEvent += (message) =>
          {
              /*if(IsDead)
                  return;*/
              _timer?.ForceEnd();
              _spriteRenderer.color = Color.white;
              _timer = new Timer(.1f).SetUpdate((x) =>
              {
                  Color color = new Color((1 - x), (1 - x), (1 - x), 1);
                  _spriteRenderer.color = color;
              });
          };
        DieEvent += (message) =>
        {
            //_timer?.ForceEnd();
            _timer = new Timer(.1f)
            .SetDelay(.1f)
            .SetUpdate((x) =>
            {
                Color color = new Color(0, 0, 0, 1 - x);
                _spriteRenderer.color = color;
            });
        };
    }

    private void OnDestroy()
    {
        _timer?.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.ReceiveDamage(new DamageMessage(this, Player, 10, DamageSources.Melee));
        }
    }

    private void Update()
    {
        if (Player != null && Player.transform != null)
            MoveTo(Player.transform.position);
    }
}
