using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Umb16.Extensions;
using Cysharp.Threading.Tasks;

public class MobGenerator : MonoBehaviour
{
    public static MobGenerator Self;
    public readonly HashSet<IMob> Mobs = new HashSet<IMob>();

    public AssetReference[] MobPrafab;

    private int _currenWave;

    private float[][] _waves = new float[10][]
    {
        new float[9]
        {
            0.84f,
            0f,
            0f,
            0f,
            0f,
            0f,
            0f,
            0.001f,
            0.05f
        },
        new float[9]
        {
            0.66f,
            33f,
            0f,
            0f,
            0f,
            0f,
            0f,
            0.001f,
            0f
        },
        new float[9]
        {
            0.49f,
            0.4f,
            0.1f,
            0f,
            0f,
            0f,
            0f,
            0.001f,
            0f
        },
        new float[9]
        {
            0.39f,
            0.4f,
            0.1f,
            0.1f,
            0f,
            0f,
            0f,
            0.001f,
            0f
        },
        new float[9]
        {
            0.29f,
            0.3f,
            0.1f,
            0.1f,
            0.2f,
            0f,
            0f,
            0.001f,
            0f
        },
        new float[9]
        {
            0.19f,
            0.2f,
            0.2f,
            0.2f,
            0.1f,
            0.01f,
            0f,
            0.001f,
            0f
        },
        new float[9]
        {
            0.19f,
            0.2f,
            0.1f,
            0.2f,
            0.1f,
            0.02f,
            0.1f,
            0.001f,
            0f
        },
        new float[9]
        {
            0.2f,
            0.1f,
            0.1f,
            0.1f,
            0.1f,
            0.2f,
            0.03f,
            0.1f,
            0f
        },
        new float[9]
        {
            0f,
            0f,
            0f,
            0f,
            0f,
            0f,
            0f,
            1f,
            0f
        },
        new float[9]
        {
            0.1f,
            0.1f,
            0.1f,
            0.1f,
            0.1f,
            0.1f,
            0.1f,
            0.01f,
            0.2f
        }
    };

    private int[] _wawesMobCount = new int[10]
    {
        20,
        20,
        20,
        25,
        35,
        40,
        45,
        50,
        55,
        60
    };

    private int currentLoop;

    private int _createMobsLeft = 20;

    private int _waveMobCounter;

    private float _time;

    private float _bossTime;
    private Player _player;

    public int WaveMobCounter
    {
        get
        {
            return _waveMobCounter;
        }
        set
        {
            _waveMobCounter = value;
            if (_waveMobCounter >= _wawesMobCount[_currenWave] * (currentLoop + 1))
            {
                NextWave();
            }
        }
    }

    [Inject]
    private void Construct(DamageController damageController, Player player)
    {
        damageController.Die += OnEnemyDie;
        _player = player;
    }

    private void OnEnemyDie(DamageMessage msg)
    {
        Mobs.Remove(msg.Target);
    }

    private void NextWave()
    {
        _currenWave++;
        if (_currenWave >= _waves.Length)
        {
            _currenWave = 0;
            currentLoop++;
        }
        _waveMobCounter = 0;
        _createMobsLeft = _wawesMobCount[_currenWave] * (currentLoop + 1);
    }

    private async UniTask CrateMob()
    {
        if (_createMobsLeft > 0)
        {
            _createMobsLeft--;
            Vector3 vector = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f);
            vector.Normalize();
            vector *= 25f;
            vector += _player.transform.position;
            GameObject gameObject = await MobPrafab[GetRandomMob()].InstantiateAsync(new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity).Task;
            //GameObject gameObject = Instantiate(MobPrafab[GetRandomMob()], new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity) as GameObject;
            Mob mob = gameObject.GetComponent<Mob>();
            //mob.Init();
            mob.AddPerk((x) => new PerkEWave(x));
            Mobs.Add(mob);
            /*if (Random.value < 0.05f)
                mob.AddPerk((x) => new PerkEChampion(x));*/
        }
        WaveMobCounter++;
        float delay = (Random.value + 2f) / (Mathf.Abs(Mathf.Sin(((float)_player.Exp.Value + _time) / 100f)) + 1f);
        //Debug.Log("Create mob delay "+ delay);
        Invoke("CrateMob", delay);
    }

    private async UniTask CreateBoss()
    {
        GetComponent<AudioSource>().Play();
        Vector3 vector = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f);
        vector.Normalize();
        vector *= 25f;
        vector += _player.transform.position;
        GameObject gameObject = await MobPrafab[Random.Range(0, MobPrafab.Length)].InstantiateAsync(new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity).Task;
        // Object.Instantiate(MobPrafab[Random.Range(0, MobPrafab.Length)], new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity) as GameObject;
        Mob mob = gameObject.GetComponent<Mob>();
        Mobs.Add(mob);
        mob.AddPerk((x) => new PerkEBoss(x));
    }

    private int GetRandomMob()
    {
        float value = Random.value;
        float num = 0f;
        for (int i = 0; i < _waves[_currenWave].Length; i++)
        {
            if (value > num && value < num + _waves[_currenWave][i])
            {
                return i;
            }
            num += _waves[_currenWave][i];
        }
        return 0;
    }

    private void Start()
    {
        Self = this;
        _createMobsLeft = _wawesMobCount[0];
        Invoke("CrateMob", 1f);
    }

    private void Update()
    {
        _bossTime += Time.deltaTime;
        if (_bossTime > 100f)
        {
            CreateBoss();
            _bossTime = 0f;
        }
        _time += Time.deltaTime;

        foreach (var mob in Mobs)
        {
            Vector3 dir = mob.Position - _player.Position;
            if ((dir).SqrMagnetudeXY() > 60 * 60)
            {
                mob.SetPosition(_player.Position + dir.NormalizedXY() * 25);
            }
        }
    }
}
