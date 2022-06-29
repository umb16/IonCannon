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
        //LiquidsControl.Add(this);
        //new Timer(Random.value * 2).SetEnd(() => _animator.enabled = true);
        HP = new ComplexStat(10);
        StatsCollection = new StandartStatsCollection(new (StatType type, ComplexStat stat)[]
        {
            (StatType.HP,new ComplexStat(10)),
        });
        Defence = StatsCollection.GetStat(StatType.Defence);
        Vector3 vector = transform.position;
        vector.z = 100;
        transform.position = vector;
        _arrivalTimer = new Timer(.2f)
            .SetUpdate(x => transform.localScale = new Vector3(1.5f, x + .5f, 1));
        _deathTimer = new Timer(Random.Range(10, 300)).SetEnd(SelfDestroy);

        AddPerk(new PerkAura(PerkType.ESlowAura, PerkType.ESlowAuraEffect,
            new[] { new StatModificator(-.5f, StatModificatorType.Multiplicative, StatType.MovementSpeed) },
            3, true));
    }

    private void FixedUpdate()
    {
        if (!IsDead)
            TargetPos = (Vector3)(Vector2)LiquidTest.Instance.xxxx.Results[Index] + Vector3.forward * 100;
    }

    public void SelfDestroy()
    {
        ReceiveDamage(new DamageMessage(this, this, 9999, DamageSources.Self));
    }

    protected override void Update()
    {
        if (!IsDead)
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position,(Vector3)Target+Vector3.forward*100, Time.deltaTime);
        //transform.position += (Vector3)Target*Time.deltaTime;
        /*Vector2 gravity = Vector2.zero;
        Vector2 repulsion = Vector2.zero;
        float countG = 0;
        float countR = 0;
        foreach (var item in _liquidsList)
        {
            if (item.ID != ID)
            {
                Vector2 vector = (item.BackedPosition - BackedPosition);
                var sqrDistance = vector.sqrMagnitude;
                if (sqrDistance == 0)
                {
                    vector = new Vector2(Random.value, Random.value);
                    sqrDistance = .1f;
                }
                if (sqrDistance < _bigRadius * _bigRadius)
                {
                    if (sqrDistance < _repulsionCurrentRadius * _repulsionCurrentRadius)
                    {
                        float k = sqrDistance / (_repulsionCurrentRadius * _repulsionCurrentRadius);
                        countR++;
                        repulsion += vector * -1 / sqrDistance * (1 - k);
                    }
                    else
                    {
                        countG++;
                        gravity += vector / sqrDistance * _gravityK;
                    }
                }
            }
        }
        _repulsionCurrentRadius = Mathf.Lerp(_repulsionMinRadius, _repulsionMaxRadius, countR * .333f);

        if (countR > 0)
            gravity /= countR;
        if (!((Vector3)(gravity + repulsion)).EqualsWithThreshold(Vector3.zero, .1f))
            transform.position += ((Vector3)(gravity + repulsion)) * Time.deltaTime;      */
    }

    /*public override void ReceiveDamage(DamageMessage message)
    {
        base.ReceiveDamage(message);
    }*/
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
