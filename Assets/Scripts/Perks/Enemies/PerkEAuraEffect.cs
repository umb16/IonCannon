using UnityEngine;

public class PerkAuraEffect : WithId, IPerk
{
    public PerkType Type { get; private set; }

    public bool IsCommon => false;

    private Fx _perkFx;
    //private float _speedUpValue = -.5f;
    private IStatModificator[] _modificators;
    private IMob _mob;

    //private Fx _speedUpfx = new Fx("Fx_SpeedUp", FxPosition.Ground);

    public PerkAuraEffect(PerkType perkType,Fx perkFx, IStatModificator[] modificators)
    {
        Type = perkType;
        _perkFx = perkFx;
        _modificators = modificators;
    }

    public void Init(IMob mob)
    {
        _mob = mob;
        //_modificator = new StatModificator(_speedUpValue, StatModificatorType.Multiplicative, StatType.MovementSpeed);
        _mob.StatsCollection.AddModificators(_modificators);
        if (_perkFx != null)
            mob.AddFx(_perkFx);
    }

    public void Shutdown()
    {
        if (_perkFx != null)
            _mob.RemoveFx(_perkFx);
        _mob.StatsCollection.RemoveModificators(_modificators);
    }

    public void Add(IPerk perk)
    {
        //throw new System.NotImplementedException();
    }

    public string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
