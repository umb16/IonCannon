using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : Mob
{
    [SerializeField] Animator _animator;
    //[SerializeField] float _speedFactor = 3;

    private ComplexStat _maxPathLength;
    private ComplexStat _raySpeed;
    private ComplexStat _rayDelay;
    private ComplexStat _raySplashRadius;
    private ComplexStat _rayDamage;

    private int _requedScore = 40;

    public GameObject Barrel;

    public GameObject GameOver;

    private float BarrelTimer;

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
        
        damageController.Die += x => Score.CurrentScore += x.Target.StatsCollection.GetStat(StatType.Score).IntValue;
        _perksMenu = perksMenu;
    }

    private void Awake()
    {
        _rayDamage = StatsCollection.GetStat(StatType.RayDamage);
        _maxPathLength = StatsCollection.GetStat(StatType.RayPathLenght);
        _raySpeed = StatsCollection.GetStat(StatType.RaySpeed);
        _rayDelay = StatsCollection.GetStat(StatType.RayDelay);
        _raySplashRadius = StatsCollection.GetStat(StatType.RayDamageAreaRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mob" || collision.gameObject.tag == "Ray")
        {
            MainMenu.gameIsStart = false;
            GameOver.SetActive(value: true);
            UnityEngine.Object.Destroy(base.gameObject);
            new Timer(1f)
                .SetUpdate((x) =>
            {
                if (this != null)
                    Time.timeScale = 1f - x;
            });
            UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(Blood, transform.position + Vector3.back * 0.5f, Blood.transform.rotation), 10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ray" || collision.gameObject.tag == "Barrel")
        {
            GameOver.SetActive(value: true);
            UnityEngine.Object.Destroy(base.gameObject);
            MainMenu.gameIsStart = false;
            new Timer(1).SetUpdate((x) =>
            {
                if (this != null)
                    Time.timeScale = 1f - x;
            });
            UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(Blood, transform.position + Vector3.back * 0.5f, Blood.transform.rotation), 10f);
        }
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
        if (!MainMenu.gameIsStart)
        {
            return;
        }
        CheckBarrel();
        ComboCalc();
        SetPerkUpMenuAction();
        CheckLvlup();
        Movement();
        //MovementSpeed.SetBaseValue(_speedFactor);
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

    private void CheckLvlup()
    {
        if (Score.CurrentScore >= _requedScore)
        {
            _requedScore += (int)(_requedScore * 1.5f);
            _perksMenu.Show();
        }
    }

    private void SetPerkUpMenuAction()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1) && avaliablePercs.Count > 0)
        {
            SetPerc = () =>
            {
                MassCurrentPerks[avaliablePercs[0]]++;
            };
            StopScore();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && avaliablePercs.Count > 1)
        {
            SetPerc = delegate
            {
                MassCurrentPerks[avaliablePercs[1]]++;
            };
            StopScore();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && avaliablePercs.Count > 2)
        {
            SetPerc = delegate
            {
                MassCurrentPerks[avaliablePercs[2]]++;
            };
            StopScore();
        }*/
    }

    private static void ComboCalc()
    {
        /* if (MobOld.ComboTimer < 1f)
         {
             MobOld.ComboTimer += Time.deltaTime;
             if (MobOld.ComboTimer >= 1f)
             {
                 MobOld.ComboCount = 0;
             }
         }*/
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
