using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umb16.Extensions;
using System;
using Random = UnityEngine.Random;

public class Liquid : Mob
{
    [SerializeField] public float _colliderRadius;
    public int Index { get; private set; }
    private Timer _deathTimer;
    private Timer _arrivalTimer;
    private Vector3 TargetPos;
    protected override void Start()
    {
        Index = LiquidTest.Instance.Add(this);
        HP = new ComplexStat(10);
        StatsCollection = new StandardStatsCollection(new (StatType type, ComplexStat stat)[]
        {
            (StatType.HP,new ComplexStat(10)),
        });
        Defence = StatsCollection.GetStat(StatType.Defence);
        Vector3 vector = transform.position;
        vector.z = 0;
        transform.position = vector;
        _arrivalTimer = new Timer(.2f)
            .SetUpdate(x => transform.localScale = new Vector3(1.5f, x + .5f, 1));
        _deathTimer = new Timer(Random.Range(10, 300)).SetEnd(SelfDestroy);

        AddPerk(new PerkAura(PerkType.ESlowAura, PerkType.ESlowAuraEffect,
            new[] { new StatModificator(-.5f, StatModificatorType.Multiplicative, StatType.MovementSpeed) },
            3, true));
    }

    protected  override void FixedUpdate()
    {
        if (!IsDead)
            TargetPos = (Vector3)(Vector2)LiquidTest.Instance.ForcesJob.Results[Index];
    }
    public void SelfDestroy()
    {
        ReceiveDamage(new DamageMessage(this, this, 9999, DamageSources.Self));
    }

    protected override void Update()
    {
        if (!IsDead)
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime);
    }

    public override void Die(DamageMessage message)
    {
        base.Die(message);
        _deathTimer?.Stop();
        _arrivalTimer?.Stop();
        LiquidTest.Instance.Remove(this);
        _deathTimer = new Timer((message.DamageSource == DamageSources.Self) ? 10 : .5f)
            .SetUpdate(x => _spriteRenderer.color *= new Color(1, 1, 1, 1 - x))
            .SetEnd(() => Destroy(gameObject));
    }

    protected override void OnDestroy()
    {
        _arrivalTimer?.Stop();
        _deathTimer?.Stop();
        LiquidTest.Instance.Remove(this);
        base.OnDestroy();
    }
}
