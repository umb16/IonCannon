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

    //LevelEvent[] _levelEvents = {

    //    new SpawnEvent(0, 960, Addresses.Mob_First, 1).SetDirection(0,360),
    //    new SpawnEvent(0, 480, Addresses.Mob_Artillery, 10).SetDirection(0,360),
    //    new SpawnEvent(0, 480, Addresses.Mob_Slowdowner, 5).SetDirection(0,360),   
    //};

    LevelEvent[] _levelEvents = {

        new SpawnEvent(0, 20, Addresses.Mob_First, 2).SetDirection(0,360),
        new SpawnEvent(21, 240, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(120, 200, Addresses.Mob_SpdBuff, 20).SetDirection(0,360),
        new SpawnEvent(210, 240, Addresses.Mob_SpdBuff, 15).SetDirection(0,360),

        new SpawnEvent(90, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(150, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(210, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
       
        //после 4 минуты

        new SpawnEvent(241, 300, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(300, 480, Addresses.Mob_First, 0.6f).SetDirection(0,360),
        new SpawnEvent(261, 480, Addresses.Mob_Child, 7).SetDirection(0,360),
        new SpawnEvent(260, 480, Addresses.Mob_Artillery, 10).SetDirection(0,360),

        new SpawnEvent(330, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(390, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(450, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),

        //после 8 минуты

        new SpawnEvent(481, 720, Addresses.Mob_Child, 2).SetDirection(0,360),
        new SpawnEvent(500, 720, Addresses.Mob_Second, 8).SetDirection(0,360),

        new SpawnEvent(540, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(600, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),
        new SpawnEvent(660, Addresses.Mob_First).SetFixedCount(40).SetDirection(0,360),

        //после 9 минуты

        new SpawnEvent(721, 960, Addresses.Mob_First, 1).SetDirection(0,360),
        new SpawnEvent(735, 960, Addresses.Mob_Artillery, 10).SetDirection(0,360),
        new SpawnEvent(735, 960, Addresses.Mob_Slowdowner, 6).SetDirection(0,360),

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
        if (msg.Target is IMob target)
            Mobs.Remove(target);
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
        vector *= _screenDiagonal * .5f;
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
        if (GameData.State != GameState.Gameplay || Stop)
            return;

        foreach (var item in _levelEvents)
        {
            item.Update();
        }

        //������������ �����
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
