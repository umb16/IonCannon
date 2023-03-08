using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks;
using Zenject;
using System.Threading;
using Umb16.Extensions;

public class Mob : MonoBehaviour, IMob
{
    private static int idIndex;
    [SerializeField] bool _mirroringOnMove = true;
    [SerializeField] bool _reverseMirroring = false;
    [SerializeField] private Transform _groundCenterPoint;
    [SerializeField] private MobType _type;
    protected SpriteRenderer _spriteRenderer;
    public Animator Animator { get; private set; }
    private Rigidbody _rigidbody;

    private int _standartLayer;
    bool _invulnerability = false;

    public int ID { get; private set; }
    public ComplexStat MovementSpeed { get; private set; }
    public ComplexStat HP { get; protected set; }
    public ComplexStat Defence { get; protected set; }

    public bool IsDead => HP == null || HP.Value <= 0;

    private Dictionary<PerkType, List<IPerk>> _perks = new Dictionary<PerkType, List<IPerk>>();
    public StandardStatsCollection StatsCollection { get; protected set; }
    public DamageController DamageController { get; private set; }

    protected EnemyPerksDB _enemyPerksDB;

    public GameData GameData { get; private set; }
    public Player Player { get; private set; }
    public bool IsReady { get; private set; }

    public Vector3 Position => transform.position;
    public Vector3 GroundCenterPosition => _groundCenterPoint.position;
    public List<IMob> AllMobs => Spawner.Mobs;

    public MobSpawner Spawner { get; private set; }

    public MobType Type => _type;

    protected Vector2 _moveTarget;
    protected bool _stopped = true;

    private MobFxes _mobFxes = new MobFxes();
    private float _stunEndTime;
    public Inventory Inventory { get; private set; } = new Inventory();

    [Inject]
    private async UniTask Construct(DamageController damageController, GameData gameData,
        AsyncReactiveProperty<Player> player, MobSpawner mobSpawner, EnemyPerksDB enemyPerksDB)
    {
        _enemyPerksDB = enemyPerksDB;
        GameData = gameData;
        DamageController = damageController;
        IsReady = true;
        Spawner = mobSpawner;
        Animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        Inventory.ItemAdded += AddItem;
        Inventory.ItemRemoved += RemoveItem;
        GameData.GameStateChanged += GameStateChanged;
        await UniTask.WaitUntil(() => player.Value != null);
        Player = player;
        _standartLayer = gameObject.layer;
    }

    public void SetInvulnerability(bool value)
    {
        if (value)
        {
            gameObject.layer = LayerMask.NameToLayer("Nether");
        }
        else
        {
            gameObject.layer = _standartLayer;
            _spriteRenderer.enabled = true;
        }
        _invulnerability = value;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
            Destroy();
    }

    public void SetAnimVariable(string name, bool value)
    {
        Animator?.SetBool(name, value);
    }

    public void AddPerk(IPerk perk)
    {
        if (_perks.TryGetValue(perk.Type, out List<IPerk> value))
        {
            if (!perk.IsCommon)
            {
                value[0].Add(perk);
            }
            else
            {
                perk.Init(this);
                value.Add(perk);
            }
        }
        else
        {
            perk.Init(this);
            _perks.Add(perk.Type, new List<IPerk>(new[] { perk }));
        }
    }

    public void RemovePerksByType(PerkType perkType)
    {
        foreach (var item in _perks[perkType])
        {
            item.Shutdown();
        }
        _perks.Remove(perkType);
    }

    public void RemovePerk(IPerk perk)
    {
        perk.Shutdown();
        _perks[perk.Type].Remove(perk);
        if (_perks[perk.Type].Count == 0)
            _perks.Remove(perk.Type);
    }

    public bool ContainPerk(PerkType perkType)
    {
        return _perks.ContainsKey(perkType);
    }

    public void AddItem(Item item)
    {
        foreach (var perk in item.Perks)
        {
            AddPerk(perk);
        }
    }
    public void RemoveItem(Item item)
    {
        foreach (var perk in item.Perks)
        {
            RemovePerk(perk);
        }
    }

    public virtual void ReceiveDamage(DamageMessage message)
    {
        if (IsDead || _invulnerability && message.DamageSource != DamageSources.God)
            return;
        _stunEndTime = message.StunTime + Time.time;
        //if ((message.DamageSource & DamageSources.RayAll) == message.DamageSource)
        if (message.DamageSource != DamageSources.Heal)
        {
            message.Damage = Mathf.Max(1, message.Damage - Defence.Value);
        }
        DamageController.SendDamage(message);
        HP.AddBaseValue(-message.Damage);
        if (IsDead)
        {
            Die(message);
        }
    }

    public virtual void Die(DamageMessage message)
    {
        OnDie();
        DamageController.SendDie(message);
    }

    public void MoveTo(Vector2 target)
    {
        _moveTarget = target;
        _stopped = false;
    }

    public void StopMove()
    {
        _stopped = true;
    }

    public void OnDie()
    {
        _stopped = true;
        ShutdownPerks();
    }

    private void ShutdownPerks()
    {
        foreach (var perksByType in _perks)
        {
            foreach (var perk in perksByType.Value)
            {
                perk.Shutdown();
            }
        }
    }

    protected virtual void Awake()
    {
        ID = ++idIndex;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        /*var vector =  transform.eulerAngles;
        vector.x = Camera.main.transform.eulerAngles.x;
        transform.eulerAngles = vector;*/
        /*UniTaskAsyncEnumerable.EveryValueChanged(Camera.main.transform, x => x.eulerAngles.x).Subscribe(x =>
        {
            var vector = transform.eulerAngles;
            vector.x = x;
            transform.eulerAngles = vector;
        },cancellationToken: this.GetCancellationTokenOnDestroy());*/
        MovementSpeed = StatsCollection.GetStat(StatType.MovementSpeed);
        HP = StatsCollection.GetStat(StatType.HP);
        Defence = StatsCollection.GetStat(StatType.Defence);
        var size = StatsCollection.GetStat(StatType.Size);
        transform.localScale = Vector3.one * size.Value;
        transform.To2DPos();
        size.ValueChanged += (x) => transform.localScale = Vector3.one * x.Value;
    }

    protected virtual void FixedUpdate()
    {
        if (!_stopped && _stunEndTime < Time.time)
        {
            if (!transform.position.EqualsWithThreshold(_moveTarget, .1f))
            {
                if (_mirroringOnMove)
                    Flip();

                //Vector3 vector = MovementSpeed.Value * ((Vector3)_moveTarget - transform.position).normalized;

                //_rigidbody.velocity = vector;
                Vector2 pos = transform.position;
                pos += MovementSpeed.Value * Time.deltaTime * (_moveTarget - pos).normalized;
                transform.position = pos.Get2D();
            }
        }
    }
    protected virtual void Update()
    {
        // transform.To2DPos();
        if (_invulnerability)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
        }
    }
    private void Flip()
    {
        var scale = transform.localScale;
        if (_moveTarget.x - transform.position.x < 0)
        {
            _spriteRenderer.flipX = !_reverseMirroring;
            //scale.x = -1;
        }
        else if (_moveTarget.x - transform.position.x > 0)
        {
            _spriteRenderer.flipX = _reverseMirroring;
            //scale.x = 1;
        }
        transform.localScale = scale;
    }

    public void SetPosition(float x, float y)
    {
        transform.Set2DPos(x, y);
    }

    public void SetPosition(Vector3 vector)
    {
        transform.Set2DPos(vector.x, vector.y);
    }

    public async UniTask AddFx(Fx fx)
    {
        Transform parent;
        switch (fx.FxPosition)
        {
            case FxPosition.Ground:
                parent = _groundCenterPoint;
                break;
            default:
                parent = transform;
                break;
        }
        var go = await PrefabCreator.GetInstance(fx.Key, parent).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        go.transform.localPosition = Vector3.zero;
        if (fx.FxPosition == FxPosition.SpriteMesh)
        {
            var shape = go.GetComponent<ParticleSystem>().shape;
            shape.shapeType = ParticleSystemShapeType.SpriteRenderer;
            shape.spriteRenderer = _spriteRenderer;
        }
        _mobFxes.Add(fx, go);
    }

    public void RemoveFx(Fx fx)
    {
        _mobFxes.Remove(fx);
    }



    protected virtual void OnDestroy()
    {
        if (!IsDead)
            ShutdownPerks();
        GameData.GameStateChanged -= GameStateChanged;
    }

    public void AddForce(Vector2 force, ForceMode mode)
    {
        _rigidbody.AddForce(force, mode);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
