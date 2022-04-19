using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPerksDB
{
    public static IPerk SpeedPerk()
    {
        return new SimplePerk(
            new[]
            { new StatModificator(.33f, StatModificatorType.Additive, StatType.MovementSpeed) },
           PerkType.Speed);
    }
    public static IPerk RaySpeedPerk()
    {
        return new SimplePerk(
           new[]
            {new StatModificator(2, StatModificatorType.Additive, StatType.RaySpeed) },
           PerkType.RaySpeed);
    }
    public static IPerk RayPathLenghtPerk()
    {
        return new SimplePerk(
            new[]
            {new StatModificator(10, StatModificatorType.Additive, StatType.RayPathLenght) },
           PerkType.RayPathLenght);
    }
    public static IPerk RayChargeDelay()
    {
        return new SimplePerk(
            new[] {new StatModificator(-.3f, StatModificatorType.Additive, StatType.RayDelay) },
           PerkType.RayDelay);
    }
    public static IPerk RayDamageAreaRadius()
    {
        return new SimplePerk(
            new[] {new StatModificator(.3f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
           PerkType.RayDamageAreaRadius);
    }
    public static IPerk RayDamage()
    {
        return new SimplePerk(
            new[] {new StatModificator(2, StatModificatorType.Additive, StatType.RayDamage) },
           PerkType.RayDamage);
    }

    public static IPerk ExplosiveBarrel()
    {
        return new PerkPBarrels();
    }
    public static IPerk Ionization()
    {
        return new PerkPIonization();
    }
}
