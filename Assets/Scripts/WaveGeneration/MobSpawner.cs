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

    private WaveData[] _waves =
    {
        new WaveData(new[]{(0,100)},120),
        new WaveData(new[]{(0,100), (2,1)},120),
        new WaveData(new[]{(0,100), (2,2)},120),
        new WaveData(new[]{(0, 100), (1,3)},60),
        new WaveData(new[]{(0, 100), (2, 3),(1,3)},60),
        new WaveData(new[]{(0, 100), (2,10)},120),
        new WaveData(new[]{(1,10)},120),
        new WaveData(new[]{(1,10),(2,10)},120),
    };

    private WaveData CurrentWave => _waves[_currenWave];

    private AsyncReactiveProperty<Player> _player;
    private GameData _gameData;
    private DamageController _damageController;
    private float _mobSpawnTime;

    [Inject]
    private void Construct(DamageController damageController, AsyncReactiveProperty<Player> player, GameData gameData)
    {
        _damageController = damageController;
        damageController.Die += OnEnemyDie;
        _player = player;
        _gameData = gameData;
        _gameData.GameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            for (int i = 0; i < Mobs.Count; i++)
            {
                IMob mob = Mobs[i];
                mob.Destroy();
            }
            CurrentWave.Reset();
            _currenWave = 0;
            Mobs.Clear();
            foreach (var trn in transform.GetComponentsInChildren<Transform>().Skip(1))
            {
                Destroy(trn.gameObject);
            }
        }
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
        }
    }

    public async UniTask<IMob> SpawnByName(string key, Vector3 position)
    {
        if (_gameData.State != GameState.Gameplay)
            return null;
        GameObject go = await PrefabCreator.Instantiate(key, position);
        IMob mob = go.GetComponent<IMob>();
        go.transform.SetParent(transform);
        Mobs.Add(mob);
        return mob;
    }
    private async UniTask CreateMob()
    {
        if (!Stop)
        {
            if (!CurrentWave.IsEnd)
            {
                float ratio = (float)(Screen.width) / Screen.height;
                float height = 17 + 17 + 5;
                float width = height * ratio;
                Vector3 vector = Vector3.zero;
                if (Random.value < .5f)
                {
                    vector.x = (Random.value - .5f) * width * 2;
                    if (Random.value < .5f)
                    {
                        vector.y = -height / 2;
                    }
                    else
                    {
                        vector.y = height / 2;
                    }
                }
                else
                {
                    vector.y = (Random.value - .5f) * height * 2;
                    if (Random.value < .5f)
                    {
                        vector.x = -width / 2;
                    }
                    else
                    {
                        vector.x = width / 2;
                    }
                }

                vector += _player.Value.transform.position;
                GameObject gameObject = await MobPrafab[GetNextMob()].InstantiateAsync(new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity).Task;
                gameObject.transform.SetParent(transform);
                Mob mob = gameObject.GetComponent<Mob>();
                mob.AddPerk(new PerkEWave());
                Mobs.Add(mob);
            }
            if (CurrentWave.IsEnd/* && (Mobs.Count(x => x.Type == MobType.Default) < 5 || CurrentWave.TimeIsOver)*/)
                NextWave();
        }
        //float delay = (Random.value + 2f) / (Mathf.Abs(Mathf.Sin(((float)_player.Exp.Value + _time) / 100f)) + 1f);
    }

    private int GetNextMob()
    {
        return _waves[_currenWave].GetNext();
    }

    private void OnDestroy()
    {
        _damageController.Die -= OnEnemyDie;
    }
    private void Update()
    {
        if (_gameData.State != GameState.Gameplay)
            return;

        if (_mobSpawnTime < Time.time)
        {
            CreateMob().Forget();
            _mobSpawnTime = Time.time + Random.value;
        }

        //Телепортация мобов
        if (teleportation)
        {
            foreach (var mob in Mobs)
            {
                Vector3 dir = mob.Position - _player.Value.Position;
                if ((dir).SqrMagnetudeXY() > 60 * 60)
                {
                    mob.SetPosition(_player.Value.Position + dir.NormalizedXY() * 25);
                }
            }
        }
    }
}
