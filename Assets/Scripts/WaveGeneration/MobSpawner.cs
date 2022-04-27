using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Umb16.Extensions;
using Cysharp.Threading.Tasks;
using System.Linq;

public class WaveData
{
    private int[] _units;
    private int _currentIndex;
    private float _startTime = 0;
    public bool TimeIsOver => WaveTime < Time.time - _startTime;
    public float WaveTime { get; private set; } = 0;
    public int UnitsLeft => _units.Length - _currentIndex;
    public int UnitsCount => _units.Length;
    public bool IsEnd { get; private set; }

    public WaveData((int id, int count)[] units, float waveTime, bool shuffle = true)
    {
        WaveTime = waveTime;
        System.Random rnd = new System.Random();
        List<int> newUnitsList = new List<int>();
        foreach (var unit in units)
        {
            newUnitsList.AddRange(Enumerable.Repeat(unit.id, unit.count));
        }
        if (shuffle)
            _units = newUnitsList.OrderBy(x => rnd.Next()).ToArray();
        else
            _units = newUnitsList.ToArray();
    }
    public void Reset()
    {
        _currentIndex = 0;
    }

    public int GetNext()
    {
        if (_currentIndex == 0)
            _startTime = Time.time;
        if (IsEnd)
            return 0;
        int result = _units[_currentIndex];
        _currentIndex++;
        if (_currentIndex >= _units.Length)
            IsEnd = true;
        return result;
    }
}

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private bool teleportation = false;
    public bool Stop;
    public readonly List<IMob> Mobs = new List<IMob>();

    public AssetReference[] MobPrafab;

    private int _currenWave;

    private WaveData[] _waves =
    {
        new WaveData(new[]{(0,15), (2,2)},60),
        new WaveData(new[]{(0,15), (1,3)},60),
        new WaveData(new[]{(0,15), (2, 3),(1,3)},60),
        new WaveData(new[]{(0,25), (2,10)},120),
        new WaveData(new[]{(1,10)},120),
        new WaveData(new[]{(1,10),(2,10)},120),
    };

    private WaveData CurrentWave => _waves[_currenWave];

    private int currentLoop;

    private float _time;

    private float _bossTime;
    private Player _player;
    private GameData _gameData;

    /*public int WaveMobCounter
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
    }*/

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

    public void NextWave()
    {
        CurrentWave.Reset();
        _currenWave++;
        _gameData.AddWave();
        Debug.Log("Current wave " + _currenWave);
        if (_currenWave >= _waves.Length)
        {
            _currenWave = 0;
            currentLoop++;
        }
    }

    public async UniTask<IMob> SpawnByName(string key, Vector3 position)
    {
        if (_gameData.State != GameState.Gameplay)
            return null;
        GameObject go = await PrefabCreator.Instantiate(key, position);
        IMob mob = go.GetComponent<IMob>();
        Mobs.Add(mob);
        return mob;
    }
    private async UniTask CreateMob()
    {
        if (_gameData.State != GameState.Gameplay)
            return;
        if (!Stop)
        {
            if (!CurrentWave.IsEnd)
            {
                Vector3 vector = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f);
                vector.Normalize();
                vector *= 25f;
                vector += _player.transform.position;
                GameObject gameObject = await MobPrafab[GetNextMob()].InstantiateAsync(new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity).Task;
                //GameObject gameObject = Instantiate(MobPrafab[GetRandomMob()], new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity) as GameObject;
                Mob mob = gameObject.GetComponent<Mob>();
                //mob.Init();
                mob.AddPerk(new PerkEWave());
                Mobs.Add(mob);
            }
            if (CurrentWave.IsEnd && (Mobs.Count(x =>/* x != null &&*/ x.Type == MobType.Default) < 5 || CurrentWave.TimeIsOver))
                NextWave();
        }
        //float delay = (Random.value + 2f) / (Mathf.Abs(Mathf.Sin(((float)_player.Exp.Value + _time) / 100f)) + 1f);
        Invoke("CreateMob", Random.value + 1f);
    }

    private async UniTask CreateBoss()
    {
        GetComponent<AudioSource>().Play();
        Vector3 vector = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f);
        vector.Normalize();
        vector *= 25f;
        vector += _player.transform.position;
        GameObject gameObject = await MobPrafab[Random.Range(0, 4)].InstantiateAsync(new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity).Task;
        // Object.Instantiate(MobPrafab[Random.Range(0, MobPrafab.Length)], new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity) as GameObject;
        Mob mob = gameObject.GetComponent<Mob>();
        Mobs.Add(mob);
        mob.AddPerk(new PerkEBoss());
    }

    private int GetNextMob()
    {
        /*float value = Random.value * _waves[_currenWave].Sum();
        float num = 0f;
        for (int i = 0; i < _waves[_currenWave].Length; i++)
        {
            if (value > num && value < num + _waves[_currenWave][i])
            {
                return i;
            }
            num += _waves[_currenWave][i];
        }*/
        return _waves[_currenWave].GetNext();
    }

    private void Start()
    {
        Invoke("CreateMob", 1f);
    }

    private void Update()
    {
        if (_gameData.State != GameState.Gameplay)
            return;
        _bossTime += Time.deltaTime;
        if (_bossTime > 100f)
        {
            CreateBoss().Forget();
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
