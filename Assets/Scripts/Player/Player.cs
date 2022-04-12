using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : Mob
{
    private ComplexStat _maxPathLength;
    private ComplexStat _raySpeed;
    private ComplexStat _rayDelay;
    private ComplexStat _raySplashRadius;
    private ComplexStat _rayDamage;

    public GameObject Barrel;
    public PlayerExp Exp = new PlayerExp();

    public GameObject Blood;
    private PerksMenu _perksMenu;
    private ComplexStat _lifeSupport;

    //public float BarrelDelay => 25 - MassCurrentPerks[7] * 5;

    //public float Radiation => RayDmg * (float)MassCurrentPerks[6] * 0.1f;

    public float RayDmg => _rayDamage.Value;

    public float RayDelay => _rayDelay.Value;

    public float RaySplash => _raySplashRadius.Value;

    public float RaySpeed => _raySpeed.Value;

    public float MaxPathLength => _maxPathLength.Value;

    [Inject]
    private void Construct(PerksMenu perksMenu, DamageController damageController)
    {
        StatsCollection = StatsCollectionsDB.StandartPlayer();
        damageController.Die += x =>
        {
            if (ID != x.Target.ID)
                Exp.AddExp(x.Target.StatsCollection.GetStat(StatType.Score).IntValue);
        };
        _perksMenu = perksMenu;
    }

    protected override void Awake()
    {
        base.Awake();
        _rayDamage = StatsCollection.GetStat(StatType.RayDamage);
        _maxPathLength = StatsCollection.GetStat(StatType.RayPathLenght);
        _raySpeed = StatsCollection.GetStat(StatType.RaySpeed);
        _rayDelay = StatsCollection.GetStat(StatType.RayDelay);
        _raySplashRadius = StatsCollection.GetStat(StatType.RayDamageAreaRadius);
        _lifeSupport = StatsCollection.GetStat(StatType.LifeSupport);
        _lifeSupport.ValueChanged += LifeSupportValueChanged;
        Exp.LevelUp += OnLvlup;
    }

    private void LifeSupportValueChanged(ComplexStat stat)
    {
        if (stat.Value <= 0)
        {
            Die(new DamageMessage(this,this,1000,DamageSources.Unknown));
        }
    }

    public override void Die(DamageMessage message)
    {
        base.Die(message);
        Stop();
    }

    private void Stop()
    {
        Destroy(gameObject);
        Destroy(Instantiate(Blood, transform.position + Vector3.back * 0.5f, Blood.transform.rotation), 10f);
    }

    protected override void Update()
    {
        base.Update();
        Movement();
    }

    private void Movement()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            dir += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            dir += Vector3.down;

        if (Input.GetKey(KeyCode.A))
            dir += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            dir += Vector3.right;
        if (dir != Vector3.zero)
        {
            _animator.SetBool("Run", true);
            _animator.speed = MovementSpeed.Value / 6;
        }
        else
        {
            _animator.speed = 1;
            _animator.SetBool("Run", false);
        }
        MoveTo(transform.position + dir * 10);
    }

    private void OnLvlup()
    {
        _perksMenu.Show();
    }
}
