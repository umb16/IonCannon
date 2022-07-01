using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemySimple : Mob
{
    [SerializeField] private DestroyType _destroyType = DestroyType.FadeOut;
    [SerializeField] private float _hp = 1;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _defence = 0;
    [SerializeField] private float _size = 1;
    [SerializeField] private float _damagePerSecond = 1;
    [SerializeField] private bool _noTouchDamage;
    [SerializeField] private PerkType[] _startPerks;
    [SerializeField] private Drop[] _drop;
    [SerializeField] bool _fixedAnimSpeed;
    [SerializeField] AudioClip _playOnDie;
    private Timer _damageTimer;
    private Timer _dieTimer;
    private Action BehaviourMethod;
    protected override void Awake()
    {
        base.Awake();
        StatsCollection = StatsCollectionsDB.StandartEnemy();
        StatsCollection.SetStat(StatType.MaxHP, _hp);
        StatsCollection.SetStat(StatType.HP, _hp);
        StatsCollection.SetStat(StatType.MovementSpeed, _speed);
        StatsCollection.SetStat(StatType.Size, _size);
        StatsCollection.SetStat(StatType.Damage, _damagePerSecond);
        StatsCollection.SetStat(StatType.Defence, _defence);
        var movSpeed = StatsCollection.GetStat(StatType.MovementSpeed);
        movSpeed.ValueChanged += (x) =>
        {
            if (!_fixedAnimSpeed && _animator != null)
                _animator.speed = x.Ratio;
        };
    }
    public void SetBehaviour(Action action)
    {
        BehaviourMethod = action;
    }
    public override void ReceiveDamage(DamageMessage message)
    {
        OnReceiveamage();
        base.ReceiveDamage(message);
    }

    public override void Die(DamageMessage message)
    {
        SoundManager.Instance.Play(_playOnDie);
        base.Die(message);
        Stop();
    }
    protected override void Start()
    {
        base.Start();
        foreach (var perk in _startPerks)
        {
            AddPerk(_enemyPerksDB.Create(perk));
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
                   .SetEnd(AfterDie);
                break;
            case DestroyType.FadeOut:
                _dieTimer = new Timer(.1f)
            .SetDelay(.1f)
            .SetUpdate((x) =>
            {
                Color color = new Color(0, 0, 0, 1 - x);
                _spriteRenderer.color = color;
            })
            .SetEnd((AfterDie));
                break;
            default:
                break;
        }

    }

    private void AfterDie()
    {
        foreach (var drop in _drop)
        {
            drop.Release(GroundCenterPosition/*, StatsCollection.GetStat(StatType.Score).Value*/, root: transform.parent);
        }
        Destroy(gameObject);
    }

    protected override void OnDestroy()
    {

        _damageTimer?.Stop();
        _dieTimer?.Stop();
        base.OnDestroy();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_noTouchDamage)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.ReceiveDamage(new DamageMessage(this, Player, StatsCollection.GetStat(StatType.Damage).Value * Time.fixedDeltaTime, DamageSources.Melee, 0));
        }
    }

    private void StandartBehaviour()
    {
        if (Player != null && Player.transform != null)
        {
            MoveTo(Player.transform.position);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (BehaviourMethod == null)
            StandartBehaviour();
        else
            BehaviourMethod.Invoke();
        
    }
}
