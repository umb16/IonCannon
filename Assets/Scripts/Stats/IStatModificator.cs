using System;

public interface IStatModificator
{
    StatType StatType { get; }
    StatModificatorType Type { get; }
    float Value { get; }
    Func<float, float> Func { get; }
}