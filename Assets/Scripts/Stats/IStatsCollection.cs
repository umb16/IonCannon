using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatsCollection
{
    public void AddModificators(StatModificator[] modificators);
    public void RemoveModificators(StatModificator[] modificators);
    public ComplexStat GetStat(StatType statType);
    public void SetStat(StatType statType, float value);
}
