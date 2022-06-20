using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerksDB
{
    public IPerk Create(PerkType perkType)
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
                return new PerkEWave();
            case PerkType.EBoss:
                return new PerkEBoss();
            case PerkType.EChampion:
                return new PerkEChampion();
            case PerkType.ESpeedAura:
                return new PerkESpeedAura();
            case PerkType.ESpeedAuraEffect:
                return new PerkESpeedAuraEffect();
            case PerkType.EAfterDeathExplosion:
                return new PerkEAfterDeathExplosion();
            case PerkType.EHunter:
                return new PerkEHunter();
            case PerkType.EMother:
                return new PerkEMother(5, Addresses.Mob_Child);
            default:
                return null;
        }
    }
}
