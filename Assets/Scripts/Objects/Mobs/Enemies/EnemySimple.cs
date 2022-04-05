using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestroyType
{
    None,
    Delayed,
    FadeOut,
}

public class EnemySimple : Mob
{
    [SerializeField] private DestroyType _destroyType = DestroyType.FadeOut;
    [SerializeField] private float _hp = 1;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _score = 1;
    [SerializeField] private float _size = 1;
    [SerializeField] private bool _noTouchDamage;
    [SerializeField] private PerkType[] _startPerks;
    private Timer _damageTimer;
    private Timer _dieTimer;
    protected override void Awake()
    {
        base.Awake();
        StatsCollection = StatsCollectionsDB.StandartEnemy();
        StatsCollection.SetStat(StatType.MaxHP, _hp);
        StatsCollection.SetStat(StatType.HP, _hp);
        StatsCollection.SetStat(StatType.MovementSpeed, _speed);
        StatsCollection.SetStat(StatType.Score, _score);
        StatsCollection.SetStat(StatType.Size, _size);
        var movSpeed = StatsCollection.GetStat(StatType.MovementSpeed);
        movSpeed.ValueChanged += (x) => _animator.speed = x.Ratio;
    }

    public override void ReceiveDamage(DamageMessage message)
    {
        OnReceiveamage();
        base.ReceiveDamage(message);
    }

    public override void Die(DamageMessage message)
    {
        base.Die(message);
        Stop();
    }
    protected override void Start()
    {
        base.Start();
        foreach (var perk in _startPerks)
        {
            AddPerk(EnemyPerksDB.Create(perk));
        }
    }

    private void OnReceiveamage()
    {
        if (IsDead)
            return;
        _damageTimer?.ForceEnd();
        _spriteRenderer.color = Color.white;
        _damageTimer = new Timer(.1f).SetUpdate((x) =>
        {
            Color color = new Color((1 - x), (1 - x), (1 - x), 1);
            _spriteRenderer.color = color;
        });
    }

    private void Stop()
    {
        switch (_destroyType)
        {
            case DestroyType.None:
                break;
            case DestroyType.Delayed:
                _dieTimer = new Timer(.5f)
                   .SetEnd(() => Destroy(gameObject));
                break;
            case DestroyType.FadeOut:
                _dieTimer = new Timer(.1f)
            .SetDelay(.1f)
            .SetUpdate((x) =>
            {
                Color color = new Color(0, 0, 0, 1 - x);
                _spriteRenderer.color = color;
            })
            .SetEnd(() => Destroy(gameObject));
                break;
            default:
                break;
        }
        
    }

    private void OnDestroy()
    {
        _damageTimer?.Stop();
        _dieTimer?.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_noTouchDamage)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.ReceiveDamage(new DamageMessage(this, Player, 10, DamageSources.Melee, 0));
        }
    }

    private void Update()
    {
        if (Player != null && Player.transform != null)
        {
            MoveTo(Player.transform.position);
        }
    }
}
