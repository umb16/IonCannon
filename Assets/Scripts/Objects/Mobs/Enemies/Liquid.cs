using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umb16.Extensions;
using System;

public class Liquid : Mob
{
    [SerializeField] public float _colliderRadius;
    [SerializeField] private float _bigRadius;
    [SerializeField] public float _gravityK = 1;
    [SerializeField] public float _repulsionMinRadius;
    [SerializeField] public float _repulsionMaxRadius;
    [SerializeField] private float _repulsionK = 1;
    [SerializeField] public float _repulsionCurrentRadius;
    public Vector2 BackedPosition;
    public Vector2 BackedTarget;
    public Vector2 Target;
    private Timer _deathTimer;

    internal void Move()
    {
        //transform.position = Vector3.Lerp(transform.position, (Vector3)Target + Vector3.forward * 100, Time.deltaTime);
        transform.position += (Vector3)Target * Time.deltaTime;
    }

    protected override void Start()
    {
        LiquidsControl.Add(this);
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
        AddPerk(new PerkAura(PerkType.ESlowAura, PerkType.ESlowAuraEffect,
            new[] { new StatModificator(-.5f, StatModificatorType.Multiplicative, StatType.MovementSpeed) },
            3, true));
        _repulsionCurrentRadius = _repulsionMinRadius;
    }

    public void BakePosition()
    {
        BackedPosition = transform.position;
    }
    public void UnbakeDirection()
    {
        Target = BackedTarget;
       // Debug.Log(Target + " "+name);
    }

    protected override void Update()
    {

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

    public override void ReceiveDamage(DamageMessage message)
    {
        base.ReceiveDamage(message);
    }
    public override void Die(DamageMessage message)
    {
        base.Die(message);
        _deathTimer = new Timer(.5f)
            .SetUpdate(x => _spriteRenderer.color *= new Color(1, 1, 1, 1 - x))
            .SetEnd(() => Destroy(gameObject));
    }
    protected override void OnDestroy()
    {
        _deathTimer?.Stop();
        LiquidsControl.Remove(this);
        base.OnDestroy();
    }
}
