using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatsCollection
{
    public void AddModificators(IStatModificator[] modificators);
    public void RemoveModificators(IStatModificator[] modificators);
    public ComplexStat GetStat(StatType statType);
    public void SetStat(StatType statType, float value);
}
