using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umb16.Extensions;

public class Liquid : Mob
{
    [SerializeField] public float _colliderRadius;
    [SerializeField] private float _bigRadius;
    [SerializeField] private float _gravityK = 1;
    [SerializeField] private float _repulsionMinRadius;
    [SerializeField] private float _repulsionMaxRadius;
    [SerializeField] private float _repulsionK = 1;
    [SerializeField]private float _repulsionCurrentRadius;
    public Vector2 Position;
    private Timer _deathTimer;
    public static List<Liquid> _liquidsList = new List<Liquid>();
    protected override void Start()
    {
        _liquidsList.Add(this);
        new Timer(Random.value * 2).SetEnd(() => _animator.enabled = true);
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

    protected override void Update()
    {
        Position = transform.position;
        Vector2 gravity = Vector2.zero;
        Vector2 repulsion = Vector2.zero;
        float countG = 0;
        float countR = 0;
        foreach (var item in _liquidsList)
        {
            if (item.ID != ID)
            {
                Vector2 vector = (item.Position - Position);
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
        // if (countR > 1)
        //     _repulsionCurrentRadius += Time.deltaTime * countR;// 
        // else
        //     _repulsionCurrentRadius -= Time.deltaTime;
        // _repulsionCurrentRadius = Mathf.Clamp(_repulsionCurrentRadius, _repulsionMinRadius, _repulsionMaxRadius);
        //Debug.Log(_repulsionCurrentRadius);
        if (countR > 0)
            gravity /= countR;
        /*if (countG > 0)
            gravity /= countG;*/
        if (!((Vector3)(gravity + repulsion)).EqualsWithThreshold(Vector3.zero, .1f))
            transform.position += ((Vector3)(gravity + repulsion)) * Time.deltaTime;
        //base.Update();
        // transform.To2DPos(100);

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
        _liquidsList.Remove(this);
        base.OnDestroy();
    }
}
