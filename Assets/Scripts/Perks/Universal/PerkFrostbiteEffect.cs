using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;

public class PerkFrostbiteEffect : PerkBase
{
    private IMob _mob;
    private IDisposable _loop;
    private StatModificator _slowdown;
    private Fx _slowDownFx = new Fx("Fx_SlowDown", FxPosition.SpriteMesh);
    public float Countdown { get; private set; }

    public PerkFrostbiteEffect()
    {
        _slowdown = new StatModificator(-.7f, StatModificatorType.Multiplicative, StatType.MovementSpeed);
        Countdown = 4;
        SetType(PerkType.FrostbiteEffect);
    }

    public override bool IsCommon => false;

    public override void Add(IPerk perk)
    {
        PerkFrostbiteEffect effect = ((PerkFrostbiteEffect)perk);
        Countdown = effect.Countdown;
    }

    public override void Init(IMob mob)
    {
        _mob = mob;
        _mob.StatsCollection.AddModificator(_slowdown);
        mob.AddFx(_slowDownFx);
        _loop = UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(Update);
    }

    public override void Shutdown()
    {
        _mob.RemoveFx(_slowDownFx);
        _mob.StatsCollection.RemoveModificator(_slowdown);
        _loop.Dispose();
    }

    private void Update(AsyncUnit obj)
    {
        Countdown--;

        if (Countdown <= 0)
        {
            _mob.RemovePerk(this);
        }           
    }
}
