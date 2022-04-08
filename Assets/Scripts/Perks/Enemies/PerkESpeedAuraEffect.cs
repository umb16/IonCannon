using UnityEngine;

public class PerkESpeedAuraEffect : PerkEStandart
{
    public override PerkType Type => PerkType.ESpeedAuraEffect;

    private StatModificatorsCollection _modificators;
    private Fx _speedUpfx = new Fx("Fx_SpeedUp", FxPosition.Ground);
    private float _speedUpValue = 4f;


    public override void Init(IMob mob)
    {
        base.Init(mob);
        SetParent(mob);
        mob.AddFx(_speedUpfx);
    }

    private void SetParent(IMob mob)
    {
        /* _modificators = new StatModificatorsCollection
         (
             new[]
             {
                     new StatModificator((x)=>
                     {
                         if( x < _speedUpValue)
                             return _speedUpValue;
                         else
                             return x;
                     }, StatModificatorType.Correction, StatType.MovementSpeed)
             }
         );*/
        _modificators = new StatModificatorsCollection
         (
             new[]
             {
                    new StatModificator(1.8f, StatModificatorType.Additive, StatType.MovementSpeed)
             }
         );
        _modificators.AddStatsCollection(_mob.StatsCollection);
    }

    public override void Shutdown()
    {
        _mob.RemoveFx(_speedUpfx);
        _modificators.RemoveAll();
    }
}
