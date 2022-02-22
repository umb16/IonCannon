using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : Mob
{
    [SerializeField] private LayerMask _rayTaregetMask;
    public GameObject PerksMenu;

    private int[] MassCurrentPerks = new int[8];

    private int[] MassPerksMax = new int[8]
    {
        3,
        3,
        5,
        3,
        5,
        10,
        3,
        3
    };

    public static Player Self;

    private ComplexStat _speed;
    private ComplexStat _maxPathLength;
    private ComplexStat _raySpeed;
    private ComplexStat _rayDamage;

    private LineRenderer _cannonPath;

    private int _currentLineIndex;
    private float _currentPathLength;

    private List<Vector2> cannonPath = new List<Vector2>();

    private Vector3 _oldPointOfPath;

    public GameObject CannonRayPrefab;

    private GameObject _cannonRay;

    private float rayTime;



    private float _rayDelayTime;

    private int _requedScore = 40;

    public GameObject Barrel;

    public GameObject GameOver;

    private float BarrelTimer;

    public GameObject Blood;

    private bool rayIsReady = true;

    public Animator PlayerAnim;

    private List<int> avaliablePercs = new List<int>();

    private Action SetPerc;


    public float BarrelDelay => 25 - MassCurrentPerks[7] * 5;

    public float Radiation => RayDmg * (float)MassCurrentPerks[6] * 0.1f;

    public float RayDmg => _rayDamage.Value + (float)(MassCurrentPerks[5] * 2);

    public float RayDelay => 1.3f - (float)MassCurrentPerks[3] * 0.3f;

    public float RaySplash => 1f + (float)MassCurrentPerks[4] * 0.3f;

    public float Speed => _speed.Value + (float)MassCurrentPerks[0];

    public float RaySpeed => _raySpeed.Value + (float)(MassCurrentPerks[1] * 2);

    public float MaxPathLength => _maxPathLength.Value + (float)(MassCurrentPerks[2] * 10);

    private void Awake()
    {
        StatsCollection = StatsCollectionsDB.StandartPlayer();
        _rayDamage = StatsCollection.GetStat(StatType.RayDamage);
        _speed = StatsCollection.GetStat(StatType.Speed);
        _maxPathLength = StatsCollection.GetStat(StatType.RayPathLenght);
        _raySpeed = StatsCollection.GetStat(StatType.RaySpeed);

        Self = this;
        _cannonPath = GetComponent<LineRenderer>();
        avaliablePercs.Clear();
        PerksMenu.SetActive(value: false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mob" || collision.gameObject.tag == "Ray")
        {
            MainMenu.gameIsStart = false;
            GameOver.SetActive(value: true);
            UnityEngine.Object.Destroy(base.gameObject);
            new Timer((x) =>
            {
                Time.timeScale = 1f - x;
            }, null, 1f, 0f);
            UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(Blood, transform.position + Vector3.back * 0.5f, Blood.transform.rotation), 10f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ray" || collision.gameObject.tag == "Barrel")
        {
            GameOver.SetActive(value: true);
            UnityEngine.Object.Destroy(base.gameObject);
            MainMenu.gameIsStart = false;
            new Timer((x) =>
            {
                Time.timeScale = 1f - x;
            }, null, 1f, 0f);
            UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(Blood, transform.position + Vector3.back * 0.5f, Blood.transform.rotation), 10f);
        }
    }

    private void StopScore()
    {
        Time.timeScale = 1f;
        PerksMenu.SetActive(value: false);
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
        RayLogic();
    }

    private void RayLogic()
    {
        if (Input.GetMouseButton(0) && _currentPathLength < MaxPathLength && rayIsReady)
        {
            if (SetPerc != null)
            {
                SetPerc();
                SetPerc = null;
            }
            Vector3 zero = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition * Camera.main.rect.width);
            Debug.DrawRay(ray.origin, ray.direction);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _rayTaregetMask.value))
            {
                return;
            }
            zero = hitInfo.point;
            _cannonPath.SetVertexCount(_currentLineIndex + 1);
            if (_currentLineIndex > 0)
            {
                if (MaxPathLength > _currentPathLength + Vector3.Distance(zero, _oldPointOfPath))
                {
                    _currentPathLength += Vector3.Distance(zero, _oldPointOfPath);
                }
                else
                {
                    float d = MaxPathLength - _currentPathLength;
                    zero = _oldPointOfPath + (zero - _oldPointOfPath).normalized * d;
                    _currentPathLength = MaxPathLength;
                }
            }
            else
            {
                cannonPath.Clear();
            }
            cannonPath.Add(zero);
            _cannonPath.SetPosition(_currentLineIndex, zero);
            _oldPointOfPath = zero;
            _currentLineIndex++;
        }
        if (Input.GetMouseButtonUp(0) && rayIsReady)
        {
            rayIsReady = false;
            _currentLineIndex = 0;
            _currentPathLength = 0f;
            rayTime = 0f;
            _rayDelayTime = 0f;
            _cannonPath.SetVertexCount(0);
        }
        if (cannonPath.Count <= 1 || _currentLineIndex != 0)
        {
            return;
        }
        _rayDelayTime += Time.deltaTime;
        if (_rayDelayTime > RayDelay)
        {
            if (_cannonRay == null)
            {
                _cannonRay = Instantiate(CannonRayPrefab);
                _cannonRay.GetComponent<RayScript>().SetSplash(RaySplash);
            }
            rayTime += Time.deltaTime * RaySpeed;
            _cannonRay.transform.position = Vector3.Lerp(cannonPath[0], cannonPath[1], rayTime / Vector2.Distance(cannonPath[0], cannonPath[1]));
            if (rayTime > Vector2.Distance(cannonPath[0], cannonPath[1]))
            {
                rayTime -= Vector2.Distance(cannonPath[0], cannonPath[1]);
                cannonPath.RemoveAt(0);
            }
            if (cannonPath.Count < 2)
            {
                _cannonRay.GetComponent<RayScript>().Stop();
                _cannonRay = null;
                rayIsReady = true;
            }
        }
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
        MoveTo(transform.position + dir * 10);
    }

    private void CheckLvlup()
    {
        if (Score.CurrentScore >= _requedScore)
        {
            _requedScore += (int)((float)_requedScore * 1.5f);
            avaliablePercs.Clear();
            List<int> list = new List<int>();
            for (int i = 0; i < MassCurrentPerks.Length; i++)
            {
                if (MassCurrentPerks[i] < MassPerksMax[i])
                {
                    list.Add(i);
                }
            }
            int num;
            for (num = 0; num < list.Count; num++)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                avaliablePercs.Add(list[index]);
                list.RemoveAt(index);
                num--;
            }
            int[] array = new int[avaliablePercs.Count];
            for (int j = 0; j < avaliablePercs.Count; j++)
            {
                array[j] = MassCurrentPerks[avaliablePercs[j]] + 1;
            }
            PerksMenu.GetComponent<PerksText>().SetPerks(avaliablePercs.ToArray(), array);
            if (avaliablePercs.Count > 0)
            {
                Time.timeScale = 0f;
                PerksMenu.SetActive(value: true);
            }
        }
    }

    private void SetPerkUpMenuAction()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && avaliablePercs.Count > 0)
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
        }
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
        if (MassCurrentPerks[7] > 0)
        {
            BarrelTimer += Time.deltaTime;
            if (BarrelTimer > BarrelDelay)
            {
                BarrelTimer -= BarrelDelay;
                CreateBarell();
            }
        }
    }
}
