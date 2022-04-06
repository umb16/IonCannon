using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Umb16.Extensions;
using Cysharp.Threading.Tasks;
using System.Linq;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private bool teleportation = false;
    public bool Stop;
    public readonly List<IMob> Mobs = new List<IMob>();

    public AssetReference[] MobPrafab;

    private int _currenWave;

    private float[][] _waves = new float[10][]
    {
        new float[9]
        {
            0.84f,
            0.2f,
            0.1f,
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
            0.33f,
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
    private GameData _gameData;

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
    private void Construct(DamageController damageController, Player player, GameData gameData)
    {
        damageController.Die += OnEnemyDie;
        _player = player;
        _gameData = gameData;
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

    public async UniTask SpawnByName(string key, Vector3 position)
    {
        if (_gameData.State != GameState.InGame)
            return;
        GameObject go = await PrefabCreator.Instantiate(key, position);
        IMob mob = go.GetComponent<IMob>();
        Mobs.Add(mob);
    }
    private async UniTask CreateMob()
    {
        if (_gameData.State != GameState.InGame)
            return;
        if (!Stop)
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
                mob.AddPerk(new PerkEWave());
                Mobs.Add(mob);
                WaveMobCounter++;
            }
            
        }
        float delay = (Random.value + 2f) / (Mathf.Abs(Mathf.Sin(((float)_player.Exp.Value + _time) / 100f)) + 1f);
        Invoke("CreateMob", delay);
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
        mob.AddPerk(new PerkEBoss());
    }

    private int GetRandomMob()
    {
        float value = Random.value * _waves[_currenWave].Sum();
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
        _createMobsLeft = _wawesMobCount[0];
        Invoke("CreateMob", 1f);
    }

    private void Update()
    {
        if (_gameData.State != GameState.InGame)
            return;
        _bossTime += Time.deltaTime;
        if (_bossTime > 100f)
        {
            //CreateBoss().Forget();
            _bossTime = 0f;
        }
        _time += Time.deltaTime;

        //Телепортация мобов
        if (teleportation)
        {
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
}
