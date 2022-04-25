using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ItemsDB
{
    public static Item GetRandomItem()
    {
        var values = Enum.GetValues(typeof(ItemType));
        var randomEnum = (ItemType)values.GetValue(Random.Range(0,values.Length));
        return CreateByType(randomEnum);
    }
    public static Item CreateByType(ItemType type)
    {
        switch (type)
        {
            case ItemType.AdditionalDrives:
                return AdditionalDrives();
            case ItemType.ExoskeletonSpeedBooster:
                return ExoskeletonSpeedBooster();
            case ItemType.Battery:
                return Battery();
            case ItemType.Amplifier:
                return Amplifier();
            case ItemType.FocusLens:
                return FocusLens();
            case ItemType.IonizationUnit:
                return IonizationUnit();
            case ItemType.DeliveryDevice:
                return DeliveryDevice();
            case ItemType.Coprocessor:
                return Coprocessor();
            default:
                return Battery();
        }
    }
    public static Item Battery()
    {
        return new Item()
        {
            Type = ItemType.Battery,
            Name = "Батарея",
            Description = "Длинна пути +50%",
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
            Type = ItemType.AdditionalDrives,
            Name = "Дополнительные приводы",
            Description = "Скорость луча +50%",
            Cost = 10,
            Icon = AddressKeys.Ico_Laser,
            Perks = new IPerk[] 
            {
                new SimplePerk(new[] 
                { 
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RaySpeed)
                },
                PerkType.RaySpeed)
            }
        };
    }
    public static Item ExoskeletonSpeedBooster()
    {
        return new Item()
        {
            Type = ItemType.ExoskeletonSpeedBooster,
            Name = "Ускоритель экзоскелета",
            Description = "Скорость бега +0.5 п/с",
            Cost = 10,
            Icon = AddressKeys.Ico_Laser,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(0.5f, StatModificatorType.Additive, StatType.MovementSpeed)
                },
                PerkType.Speed)
            }
        };
    }
    public static Item Amplifier()
    {
        return new Item()
        {
            Type = ItemType.Amplifier,
            Name = "Усилитель",
            Description = "Длинна пути -20%\nУрон луча +20%",
            Cost = 10,
            Icon = AddressKeys.Ico_Laser,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-0.2f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(0.2f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.Amplifier)
            }
        };
    }
    
    public static Item FocusLens()
    {
        return new Item()
        {
            Type = ItemType.FocusLens,
            Unique = true,
            Name = "Фокусирующая линза",
            Description = "Урон луча +50%\nШирина луча -90%",
            Cost = 10,
            Icon = AddressKeys.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-0.9f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius),
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.FocusLens)
            }
        };
    }
    public static Item IonizationUnit()
    {
        return new Item()
        {
            Type = ItemType.IonizationUnit,
            Name = "Блок ионизации",
            Description = "Поражённые лучом враги получают периодический урон 10% в секунду",
            Cost = 10,
            Icon = AddressKeys.Ico_Battery,
            Perks = new IPerk[]
            {
                new PerkPIonization(),
            }
        };
    }
    public static Item DeliveryDevice()
    {
        return new Item()
        {
            Type = ItemType.DeliveryDevice,
            Name = "Устройство доставки",
            Description = "Раз в 20 секунд доставляет с орбиты ящик со взравчаткой",
            Cost = 10,
            Icon = AddressKeys.Ico_Battery,
            Perks = new IPerk[]
            {
                new PerkPBarrels(),
            }
        };
    }
    public static Item Coprocessor()
    {
        return new Item()
        {
            Type = ItemType.Coprocessor,
            Name = "Сопроцессор",
            Description = "Время наведения -30%",
            Cost = 10,
            Icon = AddressKeys.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-.3f, StatModificatorType.Multiplicative, StatType.RayDelay)
                },
                    PerkType.RayDelay)
            },
        };
    }
}
