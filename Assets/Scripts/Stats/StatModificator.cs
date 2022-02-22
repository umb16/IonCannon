using System;

public class StatModificator : IStatModificator
{
    public StatModificatorType Type { get; private set; }
    public float Value { get; private set; }
    public StatType StatType { get; private set; }
    public Func<float, float> Func { get; private set; }

    public StatModificator(float value, StatModificatorType modificatorType, StatType statType)
    {
        Value = value;
        Type = modificatorType;
        StatType = statType;
    }
    public StatModificator(Func<float,float> func, StatModificatorType modificatorType, StatType statType)
    {
        Type = modificatorType;
        StatType = statType;
        Func = func;
    }
}
