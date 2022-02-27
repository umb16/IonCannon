using System;

public class PerkForLvlup
{
    public IPerk Perk { get; private set; }
    public float Probability { get; private set; }
    public bool Maxed => Perk.Maxed;

    //public Func<IMob, IPerk> GeneratePerk;
    public PerkForLvlup(float probability, IPerk perk)
    {
        Perk = perk;
        Probability = probability;
    }
}
