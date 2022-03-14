using UnityEngine;

public class PerkSpeedAuraEffect : IPerk
{
    public PerkType Type => PerkType.ESpeedAuraEffect;
    public string Name => throw new System.NotImplementedException();
    public string Description => throw new System.NotImplementedException();
    private IMob _mob;
    public int Level => 1;

    public bool Maxed => true;

    public int MaxLevel => 1;
    private StatModificatorsCollection _modificators;
    private Fx _speedUpfx = new Fx("Fx_SpeedUp", FxPosition.Ground);
    private float _speedUpValue = 4f;

    public PerkSpeedAuraEffect(IMob mob)
    {
        SetParent(mob);
        mob.AddFx(_speedUpfx);
    }
    public void AddLevel()
    {
        Debug.Log("Is static perk");
    }

    public void SetLevel(int level)
    {
        Debug.Log("Is static perk");
    }

    public void SetParent(IMob mob)
    {
        if (mob == null)
        {
            _modificators.RemoveStatsCollection(_mob.StatsCollection);
        }
        _mob = mob;
        _modificators = new StatModificatorsCollection
        (
            new[]
            {
                    new StatModificator((x)=>
                    {
                        if(x<_speedUpValue)
                            return _speedUpValue;
                        else
                            return x;
                    }, StatModificatorType.Correction, StatType.MovementSpeed)
            }
        );
        _modificators.AddStatsCollection(_mob.StatsCollection);
    }

    public void Shutdown()
    {
        _mob.RemoveFx(_speedUpfx);
        _modificators.RemoveAll();
    }
}
