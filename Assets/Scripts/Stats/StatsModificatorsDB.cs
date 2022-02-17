using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatsModificatorsDB
{
    public static StatModificatorsCollection SpeedBoost()
    {
        return new StatModificatorsCollection(
            new StatModificator[]
                {
                    new StatModificator(1, StatModificatorType.Additive, StatType.Speed),
                    new StatModificator(1, StatModificatorType.Additive, StatType.MaxHP),
                }
            );
    }
}
