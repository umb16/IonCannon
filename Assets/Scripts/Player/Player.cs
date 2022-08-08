using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : Mob
{
    public ComplexStat Gold { get; private set; } = new ComplexStat(0);
    private ComplexStat _energy;
    private ComplexStat _capacity;
    private ComplexStat _energyRegen;
    private ComplexStat _raySpeed;
    private ComplexStat _rayDelay;
    private ComplexStat _raySplashRadius;
    private ComplexStat _rayDamage;
    public ComplexStat RayReverse;
    private PerksFactory _perksFactory;
    private MiningDamageReceiver _miningDamageReceiver;
    public GameObject Blood;
    private ComplexStat _lifeSupport;
    public Inventory Stash = new Inventory();

    public float RayDmg => _rayDamage.Value;

    public float RayDelay => _rayDelay.Value;

    public float RaySplash => _raySplashRadius.Value;

    public float RaySpeed => _raySpeed.Value;

    public float Energy => _energy.Value;
    public float Capacity => _capacity.Value;

    private float _baseMoveSpeed;
    private bool _regenActive = true;

    [Inject]
    private void Construct(DamageController damageController, AsyncReactiveProperty<Player> player,
        ItemsDB itemsDB, PerksFactory perksFactory, MiningDamageReceiver miningDamageReceiver)
    {
        StatsCollection = StatsCollectionsDB.StandartPlayer();
        _rayDamage = StatsCollection.GetStat(StatType.RayDamage);
        _energy = StatsCollection.GetStat(StatType.Energy);
        _raySpeed = StatsCollection.GetStat(StatType.RaySpeed);
        _rayDelay = StatsCollection.GetStat(StatType.RayDelay);
        _raySplashRadius = StatsCollection.GetStat(StatType.RayDamageAreaRadius);
        _lifeSupport = StatsCollection.GetStat(StatType.LifeSupport);
        RayReverse = StatsCollection.GetStat(StatType.RayReverse);
        _capacity = StatsCollection.GetStat(StatType.Capacity);
        _energyRegen = StatsCollection.GetStat(StatType.EnergyRegen);
        _perksFactory = perksFactory;
        _miningDamageReceiver = miningDamageReceiver;
        _lifeSupport.ValueChanged += LifeSupportValueChanged;
        _stopped = false;
        player.Value = this;
        Inventory.Add(itemsDB.ShiftSystem());
        Inventory.Add(itemsDB.EnergyAbsorber());
    }

    public void AddEnergy(float value)
    {
        _energy.AddBaseValue(value);
    }

    public void EnergyRegen(bool value)
    {
        _regenActive = value;
    }

    protected override void Awake()
    {
        base.Awake();
        /* Inventory.Add(_itemsDB.Coprocessor());
         Inventory.Add(_itemsDB.Coprocessor());*/
        //Inventory.Add(_itemsDB.IonizationUnit());
        // Inventory.Add(_itemsDB.IonizationUnit());
        /*Inventory.Add(_itemsDB.DeliveryDevice());*/
        /*Inventory.Add(_itemsDB.DivergingLens());*/
        /*Inventory.Add(_itemsDB.IonizationUnit());
        Inventory.Add(_itemsDB.IonizationUnit());
        Inventory.Add(_itemsDB.IonizationUnit());
        Inventory.Add(_itemsDB.IonizationUnit());
        

        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());
        Stash.Add(_itemsDB.IonizationUnit());*/
    }

    protected override void Start()
    {
        base.Start();
        _baseMoveSpeed = MovementSpeed.BaseValue;
        AddPerk(_perksFactory.Create<PerkEAfterDeathExplosion>(0f));
        // GameData.GameStateChanged += GameStateChanged;
    }

    /*private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            Destroy(gameObject);
        }
    }*/

    public bool AddItemDirectly(Item item)
    {
        if (Inventory.FreeSlotAvailable && !Inventory.ContainsUnique(item))
        {
            Inventory.Add(item);
            return true;
        }
        if (Stash.FreeSlotAvailable)
        {
            Stash.Add(item);
            return true;
        }
        return false;
    }

    private void LifeSupportValueChanged(ComplexStat stat)
    {
        if (stat.Value <= 0)
        {
            ReceiveDamage(new DamageMessage(this, this, 1000, DamageSources.God));
        }
    }

    public override void Die(DamageMessage message)
    {
        base.Die(message);
        Stop();
    }

    private void Stop()
    {
        _spriteRenderer.enabled = false;
        //Destroy(gameObject);
        _stopped = true;
        if (Blood != null)
            Destroy(Instantiate(Blood, transform.position + Vector3.back * 0.5f, Blood.transform.rotation), 10f);
    }

    public override void ReceiveDamage(DamageMessage message)
    {
        base.ReceiveDamage(message);
        if (message.DamageSource != DamageSources.Heal && HP.Value > 0)
            SoundManager.Instance.PlayPlayerDamage();
    }

    protected override void OnDestroy()
    {
        //GameData.GameStateChanged -= GameStateChanged;
        base.OnDestroy();
    }
    protected override void Update()
    {

        if (_capacity.Value > _energy.Value && _regenActive)
            AddEnergy(Time.deltaTime * _energyRegen.Value);
        base.Update();
        if (_stopped)
            return;
        switch (_miningDamageReceiver.Tiles.GetTileTypeByCoords(Position))
        {
            case TileType.None:
                MovementSpeed.SetBaseValue(_baseMoveSpeed);
                break;
            case TileType.Grass:
                MovementSpeed.SetBaseValue(_baseMoveSpeed * 1.1f);
                break;
            case TileType.Layer2:
                MovementSpeed.SetBaseValue(_baseMoveSpeed * 1.0f);
                break;
            case TileType.Layer3:
                MovementSpeed.SetBaseValue(_baseMoveSpeed * 0.9f);
                break;
            case TileType.Layer4:
                MovementSpeed.SetBaseValue(_baseMoveSpeed * 0.8f);
                break;
            default:
                break;
        }
        ;
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
            Animator.SetBool("Run", true);
            Animator.speed = MovementSpeed.Value / 6;
        }
        else
        {
            Animator.speed = 1;
            Animator.SetBool("Run", false);
        }
        MoveTo(transform.position + dir * 10);
    }
}
