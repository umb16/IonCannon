using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerksDB
{
    public static IPerk SpeedPerk(IStatsCollection statsCollection)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
           new[]
           {
            new[] {new StatModificator(1, StatModificatorType.Additive, StatType.Speed) },
            new[] {new StatModificator(2, StatModificatorType.Additive, StatType.Speed) },
            new[] {new StatModificator(3, StatModificatorType.Additive, StatType.Speed) },
           },
           statsCollection);
    }
    public static IPerk RaySpeedPerk(IStatsCollection statsCollection)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
           new[]
           {
            new[] {new StatModificator(2, StatModificatorType.Additive, StatType.RaySpeed) },
            new[] {new StatModificator(4, StatModificatorType.Additive, StatType.RaySpeed) },
            new[] {new StatModificator(6, StatModificatorType.Additive, StatType.RaySpeed) },
           },
           statsCollection);
    }
    public static IPerk RayPathPerk(IStatsCollection statsCollection)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
           new[]
           {
            new[] {new StatModificator(10, StatModificatorType.Additive, StatType.RayPathLenght) },
            new[] {new StatModificator(20, StatModificatorType.Additive, StatType.RayPathLenght) },
            new[] {new StatModificator(30, StatModificatorType.Additive, StatType.RayPathLenght) },
            new[] {new StatModificator(40, StatModificatorType.Additive, StatType.RayPathLenght) },
            new[] {new StatModificator(50, StatModificatorType.Additive, StatType.RayPathLenght) },
           },
           statsCollection);
    }
    public static IPerk RayDelay(IStatsCollection statsCollection)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
           new[]
           {
            new[] {new StatModificator(-.3f, StatModificatorType.Additive, StatType.RayDelay) },
            new[] {new StatModificator(-.6f, StatModificatorType.Additive, StatType.RayDelay) },
            new[] {new StatModificator(-.9f, StatModificatorType.Additive, StatType.RayDelay) },
           },
           statsCollection);
    }
    public static IPerk RayDamageAreaRadius(IStatsCollection statsCollection)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
           new[]
           {
            new[] {new StatModificator(.3f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
            new[] {new StatModificator(.6f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
            new[] {new StatModificator(.9f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
            new[] {new StatModificator(1.2f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
            new[] {new StatModificator(1.5f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
           },
           statsCollection);
    }
    public static IPerk RayDamage(IStatsCollection statsCollection)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.Speed),
           new[]
           {
            new[] {new StatModificator(2, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(4, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(6, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(8, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(10, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(12, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(14, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(16, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(18, StatModificatorType.Additive, StatType.RayDamage) },
            new[] {new StatModificator(20, StatModificatorType.Additive, StatType.RayDamage) },
           },
           statsCollection);
    }
}
