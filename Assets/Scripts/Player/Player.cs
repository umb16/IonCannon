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

    private void Awake()
    {
        _rayDamage = StatsCollection.GetStat(StatType.RayDamage);
        _maxPathLength = StatsCollection.GetStat(StatType.RayPathLenght);
        _raySpeed = StatsCollection.GetStat(StatType.RaySpeed);
        _rayDelay = StatsCollection.GetStat(StatType.RayDelay);
        _raySplashRadius = StatsCollection.GetStat(StatType.RayDamageAreaRadius);
        Exp.LevelUp += OnLvlup;
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
    private void CreateBarell()
    {
        Vector2 v = new Vector2(UnityEngine.Random.value * 2f - 1f, UnityEngine.Random.value * 2f - 1f);
        v.Normalize();
        v *= (float)UnityEngine.Random.Range(2, 10);
        float x = v.x;
        Vector3 position = transform.position;
        v.x = x + position.x;
        float y = v.y;
        Vector3 position2 = transform.position;
        v.y = y + position2.y;
        if (v.x > 9f)
        {
            v.x = 9f;
        }
        if (v.y > 16f)
        {
            v.y = 16f;
        }
        if (v.x < -9f)
        {
            v.x = -9f;
        }
        if (v.y < -6f)
        {
            v.y = -6f;
        }
        UnityEngine.Object.Instantiate(Barrel, v, Quaternion.identity);
    }

    private void Update()
    {
        CheckBarrel();
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

    private void CheckBarrel()
    {
        /*if (MassCurrentPerks[7] > 0)
        {
            BarrelTimer += Time.deltaTime;
            if (BarrelTimer > BarrelDelay)
            {
                BarrelTimer -= BarrelDelay;
                CreateBarell();
            }
        }*/
    }
}
