using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemsDB
{
    public static Item Battery()
    {
        return new Item()
        {
            Name = "�������",
            Description = "������ ���� +50%",
            Cost = 10,
            Icon = AddressKeys.Ico_Battery,
            Perks = new IPerk[] 
            { 
                new SimplePerk(new[] 
                { 
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RayPathLenght) 
                },
                    PerkType.RayPathLenght) 
            },
        };
    }
    public static Item AdditionalDrives()
    {
        return new Item()
        {
            Name = "�������������� �������",
            Description = "�������� ���� +100%",
            Cost = 10,
            Icon = AddressKeys.Ico_Laser,
            Perks = new IPerk[] 
            {
                new SimplePerk(new[] 
                { 
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed)
                },
                PerkType.RaySpeed)
            }
        };
    }
}
