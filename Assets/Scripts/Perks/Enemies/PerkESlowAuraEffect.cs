using UnityEngine;

public class PerkESlowAuraEffect : PerkEStandart
{
    public override PerkType Type => PerkType.ESlowAuraEffect;
    private float _speedUpValue = -.5f;
    private IStatModificator _modificator;
    //private Fx _speedUpfx = new Fx("Fx_SpeedUp", FxPosition.Ground);



    public override void Init(IMob mob)
    {
        base.Init(mob);
        _modificator = new StatModificator(_speedUpValue, StatModificatorType.Multiplicative, StatType.MovementSpeed);
        _mob.StatsCollection.AddModificator(_modificator);
        //mob.AddFx(_speedUpfx);
    }

    public override void Shutdown()
    {
        //_mob.RemoveFx(_speedUpfx);
        _mob.StatsCollection.RemoveModificator(_modificator);
    }
}
