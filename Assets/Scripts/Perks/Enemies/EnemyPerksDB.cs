using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyPerksDB
{
    public static IPerk Create(PerkType perkType, IMob mob)
    {
        switch (perkType)
        {
            /*case PerkType.Speed:
                break;
            case PerkType.RaySpeed:
                break;
            case PerkType.RayPathLenght:
                break;
            case PerkType.RayDelay:
                break;
            case PerkType.RayDamageAreaRadius:
                break;
            case PerkType.RayDamage:
                break;*/
            case PerkType.EWaveFactor:
                return new PerkEWave(mob);
            case PerkType.EBoss:
                return new PerkEBoss(mob);
            case PerkType.EChampion:
                return new PerkEChampion(mob);
            case PerkType.ESpeedAura:
                return new PerkESpeedAura(mob);
            case PerkType.ESpeedAuraEffect:
                return new PerkESpeedAuraEffect(mob);
            case PerkType.EAfterDeathExplosion:
                return new PerkEAfterDeathExplosion(mob);
            default:
                return null;
        }
    }
}
