using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerPerksController
{

    private static (float probability, Func<IMob, IPerk> createPerk)[] _perkGenerators =
    {
        (1,PlayerPerksDB.SpeedPerk),
        (1,PlayerPerksDB.RaySpeedPerk),
         (1,PlayerPerksDB.RayPathLenghtPerk),
         (1,PlayerPerksDB.RayChargeDelay),
         (1,PlayerPerksDB.RayDamageAreaRadius),
         (1,PlayerPerksDB.RayDamage),
    };
    List<PerkForLvlup> _perkForLvlups = new List<PerkForLvlup>();
    public PlayerPerksController(Player player)
    {
        foreach (var item in _perkGenerators)
        {
            _perkForLvlups.Add(new PerkForLvlup(item.probability, item.createPerk(player)));
        }
    }

    public IPerk[] GetRandomAvaliable(int number = 3)
    {
        var perksList = _perkForLvlups.Where(x => !x.Maxed).ToArray();
        if(!perksList.Any())
            return new IPerk[0];
        number = Mathf.Min(number, perksList.Count());
        IPerk[] result = new IPerk[number];

        float weightSum = 0;
        for (int i = 0; i < number; i++)
        {
            result[i] = perksList[i].Perk;
            weightSum += perksList[i].Probability;
        }
        for (int i = number; i < perksList.Length; i++)
        {
            weightSum += perksList[i].Probability;
            float probability = perksList[i].Probability / weightSum;
            if (Random.value <= probability)
            {
                result[Random.Range(0, result.Length)] = perksList[i].Perk;
            }
        }
        return result;
    }
}
