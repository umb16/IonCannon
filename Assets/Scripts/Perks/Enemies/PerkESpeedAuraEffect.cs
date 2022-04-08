using UnityEngine;

public class PerkESpeedAuraEffect : PerkEStandart
{
    public override PerkType Type => PerkType.ESpeedAuraEffect;
    private float _speedUpValue = 1.8f;
    private IStatModificator _modificator;
    private Fx _speedUpfx = new Fx("Fx_SpeedUp", FxPosition.Ground);



    public override void Init(IMob mob)
    {
        base.Init(mob);
        _modificator = new StatModificator(_speedUpValue, StatModificatorType.Additive, StatType.MovementSpeed);
        _mob.StatsCollection.AddModificator(_modificator);
        mob.AddFx(_speedUpfx);
    }

    public override void Shutdown()
    {
        _mob.RemoveFx(_speedUpfx);
        _mob.StatsCollection.RemoveModificator(_modificator);
    }
}
