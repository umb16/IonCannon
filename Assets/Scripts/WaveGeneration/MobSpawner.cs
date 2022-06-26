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

    private AsyncReactiveProperty<Player> _player;
    public GameData GameData { get; private set; }
    private DamageController _damageController;

    LevelEvent[] _levelEvents = { 
        new SpawnEvent(0, Addresses.Mob_Slowdowner).SetFixedCount(10),
        //new SpawnEvent(0, 5, Addresses.Mob_Child, 2),
        //new SpawnEvent(6, Addresses.Mob_Second),
    };
    private float _screenRatio;
    private int _screenHeight;
    private float _screenWidth;
    private float _screenDiagonal;

    [Inject]
    private void Construct(DamageController damageController, AsyncReactiveProperty<Player> player, GameData gameData)
    {
        _damageController = damageController;
        damageController.Die += OnEnemyDie;
        _player = player;
        GameData = gameData;
        GameData.GameStateChanged += OnGameStateChanged;
    }
    private void Start()
    {
        CalcScreenSpawnParams();
        foreach (var eventItem in _levelEvents)
        {
            eventItem.SetSpawner(this);
        }
    }
    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            Mobs.Clear();
            foreach (var eventItem in _levelEvents)
            {
                eventItem.Reset();
            }
            foreach (var trn in transform.GetComponentsInChildren<Transform>().Skip(1))
            {
                if (trn.GetComponent<IMob>() == null)
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
        GameData.AddWave();
    }

    public async UniTask<IMob> SpawnByName(string key, Vector3 position)
    {
        if (GameData.State != GameState.Gameplay)
            return null;
        GameObject go = await PrefabCreator.Instantiate(key, position);
        IMob mob = go.GetComponent<IMob>();
        go.transform.SetParent(transform);
        Mobs.Add(mob);
        return mob;
    }

    private void CalcScreenSpawnParams()
    {
        _screenRatio = (float)(Screen.width) / Screen.height;
        _screenHeight = 17 + 17 + 5;
        _screenWidth = _screenHeight * _screenRatio;
        _screenDiagonal = Mathf.Sqrt(_screenHeight * _screenHeight + _screenWidth * _screenWidth);
    }
    public Vector3 GetSpawnPointFromDirection(float direction)
    {

        Vector3 vector = Vector3.down;
        vector = vector.DiamondRotateXY(direction / 90);
        vector *= _screenDiagonal*.5f;
        vector += _player.Value.transform.position;
        return vector;
    }
    public Vector3 GetRandomSpawnPoint()
    {
        
        Vector3 vector = Vector3.zero;
        if (Random.value < .5f)
        {
            vector.x = (Random.value - .5f) * _screenWidth * 2;
            if (Random.value < .5f)
            {
                vector.y = -_screenHeight / 2;
            }
            else
            {
                vector.y = _screenHeight / 2;
            }
        }
        else
        {
            vector.y = (Random.value - .5f) * _screenHeight * 2;
            if (Random.value < .5f)
            {
                vector.x = -_screenWidth / 2;
            }
            else
            {
                vector.x = _screenWidth / 2;
            }
        }

        vector += _player.Value.transform.position;
        return vector;
    }

    private void OnDestroy()
    {
        _damageController.Die -= OnEnemyDie;
    }
    private void Update()
    {
        if (GameData.State != GameState.Gameplay)
            return;

        foreach (var item in _levelEvents)
        {
            item.Update();
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
