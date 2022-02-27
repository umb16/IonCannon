using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPerksDB
{
    public static IPerk SpeedPerk(IMob mob)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.MoveSpeed),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.MoveSpeed),
           new[]
           {
            new[] {new StatModificator(1, StatModificatorType.Additive, StatType.MovementSpeed) },
            new[] {new StatModificator(2, StatModificatorType.Additive, StatType.MovementSpeed) },
            new[] {new StatModificator(3, StatModificatorType.Additive, StatType.MovementSpeed) },
           },
           mob,
           PerkType.Speed);
    }
    public static IPerk RaySpeedPerk(IMob mob)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RaySpeed),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RaySpeed),
           new[]
           {
            new[] {new StatModificator(2, StatModificatorType.Additive, StatType.RaySpeed) },
            new[] {new StatModificator(4, StatModificatorType.Additive, StatType.RaySpeed) },
            new[] {new StatModificator(6, StatModificatorType.Additive, StatType.RaySpeed) },
           },
           mob,
           PerkType.RaySpeed);
    }
    public static IPerk RayPathLenghtPerk(IMob mob)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RayPathLenght),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RayPathLenght),
           new[]
           {
            new[] {new StatModificator(10, StatModificatorType.Additive, StatType.RayPathLenght) },
            new[] {new StatModificator(20, StatModificatorType.Additive, StatType.RayPathLenght) },
            new[] {new StatModificator(30, StatModificatorType.Additive, StatType.RayPathLenght) },
            new[] {new StatModificator(40, StatModificatorType.Additive, StatType.RayPathLenght) },
            new[] {new StatModificator(50, StatModificatorType.Additive, StatType.RayPathLenght) },
           },
           mob,
           PerkType.RayPathLenght);
    }
    public static IPerk RayChargeDelay(IMob mob)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RayChargeDelay),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RayChargeDelay),
           new[]
           {
            new[] {new StatModificator(-.3f, StatModificatorType.Additive, StatType.RayDelay) },
            new[] {new StatModificator(-.6f, StatModificatorType.Additive, StatType.RayDelay) },
            new[] {new StatModificator(-.9f, StatModificatorType.Additive, StatType.RayDelay) },
           },
           mob,
           PerkType.RayDelay);
    }
    public static IPerk RayDamageAreaRadius(IMob mob)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RayDamageAreaRadius),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RayDamageAreaRadius),
           new[]
           {
            new[] {new StatModificator(.3f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
            new[] {new StatModificator(.6f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
            new[] {new StatModificator(.9f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
            new[] {new StatModificator(1.2f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
            new[] {new StatModificator(1.5f, StatModificatorType.Additive, StatType.RayDamageAreaRadius) },
           },
           mob,
           PerkType.RayDamageAreaRadius);
    }
    public static IPerk RayDamage(IMob mob)
    {
        return new SimplePerk(
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RayDamage),
            () => LocalizationManager.Instance.GetPhrase(LocKeys.RayDamage),
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
           mob,
           PerkType.RayDamage);
    }
}