using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsDB
{
    private ItemType[] shop ={
        ItemType.AdditionalDrives,
        ItemType.ExoskeletonSpeedBooster,
         ItemType.Battery,
         ItemType.Amplifier,
         ItemType.FocusLens,
         ItemType.IonizationUnit,
         ItemType.DeliveryDevice,
         ItemType.Coprocessor,
         ItemType.DivergingLens,
         ItemType.MagneticManipulator,
    };
    private PerksFactory _perksFactory;

    private ItemsDB(PerksFactory perksFactory)
    {
        _perksFactory = perksFactory;
    }

    public Item GetRandomItem()
    {
        var randomEnum = shop[Random.Range(0, shop.Length)];
        return CreateByType(randomEnum);
    }
    public Item CreateByType(ItemType type)
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
            case ItemType.DivergingLens:
                return DivergingLens();
            case ItemType.PowerController:
                return PowerController();
            case ItemType.LensSystem:
                return LensSystem();
            case ItemType.MagneticManipulator:
                return MagneticManipulator();
            case ItemType.SpeedDrives:
                return SpeedDrives();
            case ItemType.CoprocessorPlus:
                return CoprocessorPlus();
            case ItemType.AmplifierPlus:
                return AmplifierPlus();
            case ItemType.AmplifierPlusPlus:
                return AmplifierPlusPlus();
            case ItemType.LensSystemPlus:
                return LensSystemPlus();
            case ItemType.BatteryPlus:
                return BatteryPlus();
            case ItemType.BatteryPlusPlus:
                return BatteryPlusPlus();
            case ItemType.DeliveryDevicePlus:
                return DeliveryDevicePlus();
            case ItemType.DeliveryDevicePlusPlus:
                return DeliveryDevicePlusPlus();
            case ItemType.CoprocessorPlusPlus:
                return CoprocessorPlusPlus();
            case ItemType.IonizationUnitPlus:
                return IonizationUnitPlus();
            case ItemType.IonizationUnitPlusPlus:
                return IonizationUnitPlusPlus();
            case ItemType.PowerControllerPlus:
                return PowerControllerPlus();
            case ItemType.PowerControllerPlusPlus:
                return PowerControllerPlusPlus();
            case ItemType.ShiftSystem:
                return ShiftSystem();
            case ItemType.ReverseSystem:
                return ReverseSystem();
            case ItemType.None:
            default:
                return Battery();
        }
    }
    public Item Battery()
    {
        return new Item()
        {
            Type = ItemType.Battery,
            Name = "Батарея",
            Description = "Длинна пути +50%",
            Cost = 30,
            Icon = Addresses.Ico_Battery,
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
    public Item BatteryPlus()
    {
        return new Item()
        {
            Type = ItemType.BatteryPlus,
            Name = "Батарея+",
            UpgradeCount = 1,
            Description = "Длинна пути +75%",
            Cost = 75,
            Icon = Addresses.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.75f, StatModificatorType.Multiplicative, StatType.RayPathLenght)
                },
                    PerkType.RayPathLenght)
            },
        };
    }
    public Item BatteryPlusPlus()
    {
        return new Item()
        {
            Type = ItemType.BatteryPlusPlus,
            Name = "Батарея++",
            UpgradeCount = 2,
            Description = "Длинна пути +150%",
            Cost = 75,
            Icon = Addresses.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.RayPathLenght)
                },
                    PerkType.RayPathLenght)
            },
        };
    }
    public Item AdditionalDrives()
    {
        return new Item()
        {
            Type = ItemType.AdditionalDrives,
            Name = "Дополнительные приводы",
            Description = "Скорость луча +50%",
            Cost = 50,
            Icon = Addresses.Ico_Servo,
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
    public Item ExoskeletonSpeedBooster()
    {
        return new Item()
        {
            Type = ItemType.ExoskeletonSpeedBooster,
            Name = "Ускоритель экзоскелета",
            Description = "Скорость бега +0.5 п/с",
            Cost = 60,
            Icon = Addresses.Ico_SpeedBoost,
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
    public Item Amplifier()
    {
        return new Item()
        {
            Type = ItemType.Amplifier,
            Name = "Усилитель",
            Description = "Длинна пути -20%\nУрон луча +20%",
            Cost = 50,
            Icon = Addresses.Ico_Amplifier,
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
    public Item AmplifierPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.AmplifierPlus,
            Name = "Усилитель+",
            Description = "Длинна пути -30%\nУрон луча +40%",
            Cost = 150,
            Icon = Addresses.Ico_Amplifier,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-0.3f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(0.4f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.Amplifier)
            }
        };
    }
    public Item AmplifierPlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.AmplifierPlusPlus,
            Name = "Усилитель+",
            Description = "Длинна пути -40%\nУрон луча +100%",
            Cost = 400,
            Icon = Addresses.Ico_Amplifier,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-0.4f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.Amplifier)
            }
        };
    }
    public Item FocusLens()
    {
        return new Item()
        {
            Type = ItemType.FocusLens,
            Unique = true,
            Name = "Фокусирующая линза",
            Description = "Урон луча +50%\nШирина луча -90%",
            Cost = 70,
            Icon = Addresses.Ico_Lens,
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
    public Item IonizationUnit()
    {
        return new Item()
        {
            Type = ItemType.IonizationUnit,
            Name = "Блок ионизации",
            Description = "Поражённые лучом враги получают 4 урона в секунду в течение 10 секунд",
            Cost = 60,
            Icon = Addresses.Ico_Radiation,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>(4f, 10f)//урон, длительность
            }
        };
    }
    public Item IonizationUnitPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.IonizationUnitPlus,
            Name = "Блок ионизации+",
            Description = "Поражённые лучом враги получают 6 урона в секунду в течение 15 секунд",
            Cost = 180,
            Icon = Addresses.Ico_Radiation,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>(6f, 15f)//урон, длительность
            }
        };
    }
    public Item IonizationUnitPlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.IonizationUnitPlusPlus,
            Name = "Блок ионизации++",
            Description = "Поражённые лучом враги получают 12 урона в секунду в течение 20 секунд",
            Cost = 480,
            Icon = Addresses.Ico_Radiation,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>(12f, 20f)//урон, длительность
            }
        };
    }
    public Item ShiftSystem()
    {
        return new Item()
        {
            Unique = true,
            Type = ItemType.ShiftSystem,
            Name = "Система сдвига",
            Description = "При получение урона делает нематериальным на 2 секунды.\nВесть входящий урон увеличивается на 1.",
            Cost = 100,
            Icon = Addresses.Ico_ShiftSystem,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPShiftSystem>(),
            }
        };
    }
    public Item DeliveryDevice()
    {
        return new Item()
        {
            Type = ItemType.DeliveryDevice,
            Name = "Устройство доставки",
            Description = "Раз в 20 секунд доставляет с орбиты ящик со взравчаткой",
            Cost = 50,
            Icon = Addresses.Ico_Box,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(20f),//cooldown
            }
        };
    }
    public Item DeliveryDevicePlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.DeliveryDevicePlus,
            Name = "Устройство доставки+",
            Description = "Раз в 15 секунд доставляет с орбиты ящик со взравчаткой",
            Cost = 150,
            Icon = Addresses.Ico_Box,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(15f),//cooldown
            }
        };
    }
    public Item DeliveryDevicePlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.DeliveryDevicePlusPlus,
            Name = "Устройство доставки++",
            Description = "Раз в 10 секунд доставляет с орбиты ящик со взравчаткой",
            Cost = 400,
            Icon = Addresses.Ico_Box,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(10f),//cooldown
            }
        };
    }
    public Item Coprocessor()
    {
        return new Item()
        {
            Type = ItemType.Coprocessor,
            Name = "Сопроцессор",
            Description = "Время наведения -30%\nПогрешность -1 п",
            Cost = 50,
            Icon = Addresses.Ico_Chip,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-.3f, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-1f, StatModificatorType.Additive, StatType.RayError),
                },
                    PerkType.RayDelay)
            },
        };
    }
    public Item CoprocessorPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.CoprocessorPlus,
            Name = "Сопроцессор+",
            Description = "Время наведения -50%\nПогрешность -2 п",
            Cost = 150,
            Icon = Addresses.Ico_Chip,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-.5f, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-2f, StatModificatorType.Additive, StatType.RayError),
                },
                    PerkType.RayDelay)
            },
        };
    }

    public Item CoprocessorPlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.CoprocessorPlusPlus,
            Name = "Сопроцессор++",
            Description = "Время наведения -100%\nПогрешность -4 п",
            Cost = 400,
            Icon = Addresses.Ico_Chip,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(-1, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-4f, StatModificatorType.Additive, StatType.RayError),
                },
                    PerkType.RayDelay)
            },
        };
    }

    public Item DivergingLens()
    {
        return new Item()
        {
            Type = ItemType.DivergingLens,
            Unique = false,
            Name = "Рассеивающая линза",
            Description = "Урон луча -50%\nШирина луча +200%",
            Cost = 60,
            Icon = Addresses.Ico_DivergingLens,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(2f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius),
                    new StatModificator(-0.5f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                PerkType.DivergingLens)
            }
        };
    }
    public Item PowerController()
    {
        return new Item()
        {
            Type = ItemType.PowerController,
            NotForSale = true,
            Name = "Контроллер питания",
            Description = "Урон +20%\nДлинна пути +20%",
            Cost = 100,
            Icon = Addresses.Ico_PowerController,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                    PerkType.PowerController)
            },
        };
    }
    public Item PowerControllerPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.PowerControllerPlus,
            NotForSale = true,
            Name = "Контроллер питания+",
            Description = "Урон +30%\nДлинна пути +30%",
            Cost = 200,
            Icon = Addresses.Ico_PowerController,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                    PerkType.PowerController)
            },
        };
    }
    public Item PowerControllerPlusPlus()
    {
        return new Item()
        {
            UpgradeCount = 2,
            Type = ItemType.PowerControllerPlusPlus,
            NotForSale = true,
            Name = "Контроллер питания++",
            Description = "Урон +60%\nДлинна пути +60%",
            Cost = 500,
            Icon = Addresses.Ico_PowerController,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.RayPathLenght),
                    new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.RayDamage)
                },
                    PerkType.PowerController)
            },
        };
    }
    public Item LensSystem()
    {
        return new Item()
        {
            Type = ItemType.LensSystem,
            NotForSale = true,
            Unique = false,
            Name = "Система линз",
            Description = "Ширина луча +100%",
            Cost = 150,
            Icon = Addresses.Ico_Lenses,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)
                },
                PerkType.LensSystem)
            }
        };
    }
    public Item LensSystemPlus()
    {
        return new Item()
        {
            UpgradeCount = 1,
            Type = ItemType.LensSystemPlus,
            NotForSale = true,
            Unique = false,
            Name = "Система линз+",
            Description = "Ширина луча +200%",
            Cost = 400,
            Icon = Addresses.Ico_Lenses,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(2f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)
                },
                PerkType.LensSystem)
            }
        };
    }
    public Item MagneticManipulator()
    {
        return new Item()
        {
            Type = ItemType.MagneticManipulator,
            Unique = false,
            Name = "Магнитный манипулятор",
            Description = "Радиус сбора + 100%",
            Cost = 40,
            Icon = Addresses.Ico_Magnet,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.PickupRadius)
                },
                PerkType.MagneticManipulator)
            }
        };
    }
    public Item ReverseSystem()
    {
        return new Item()
        {
            Type = ItemType.ReverseSystem,
            Name = "Система обратного хода.",
            Description = "Скорость луча +100%\nЛуч движется туда и обратно.",
            Cost = 300,
            Icon = Addresses.Ico_Reverse,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1f, StatModificatorType.Additive, StatType.RayReverse),
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed)
                },
                PerkType.RayReverse)
            }
        };
    }
    public Item SpeedDrives()
    {
        return new Item()
        {
            Type = ItemType.SpeedDrives,
            Name = "Скоростные приводы",
            Description = "Скорость луча +100%\nПогрешность +3 п",
            Cost = 100,
            Icon = Addresses.Ico_SpeedServo,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed),
                    new StatModificator(3, StatModificatorType.Additive, StatType.RayError),
                },
                PerkType.RaySpeed)
            }
        };
    }
}
