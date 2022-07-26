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
        return new Item(LocaleKeys.Main.i_Battery)
        {
            Type = ItemType.Battery,
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
        return new Item(LocaleKeys.Main.i_Battery)
        {
            Type = ItemType.BatteryPlus,
            UpgradeCount = 1,
            Cost = 75,
            Icon = Addresses.Ico_BatteryP,
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
        return new Item(LocaleKeys.Main.i_Battery)
        {
            Type = ItemType.BatteryPlusPlus,
            UpgradeCount = 2,
            Cost = 75,
            Icon = Addresses.Ico_BatteryPP,
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
        return new Item(LocaleKeys.Main.i_AdditionalDrives)
        {
            Type = ItemType.AdditionalDrives,
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
        return new Item(LocaleKeys.Main.i_ExoskeletonSpeedBooster)
        {
            Type = ItemType.ExoskeletonSpeedBooster,
            Cost = 60,
            Icon = Addresses.Ico_SpeedBoost,
            Perks = new IPerk[]
            {
                new SimplePerk(new[]
                {
                    new StatModificator(0.1f, StatModificatorType.Multiplicative, StatType.MovementSpeed)
                },
                PerkType.Speed)
            }
        };
    }
    public Item Amplifier()
    {
        return new Item(LocaleKeys.Main.i_Amplifier)
        {
            Type = ItemType.Amplifier,
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
        return new Item(LocaleKeys.Main.i_Amplifier)
        {
            UpgradeCount = 1,
            Type = ItemType.AmplifierPlus,
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
        return new Item(LocaleKeys.Main.i_Amplifier)
        {
            UpgradeCount = 2,
            Type = ItemType.AmplifierPlusPlus,
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
        return new Item(LocaleKeys.Main.i_FocusLens)
        {
            Type = ItemType.FocusLens,
            Unique = true,
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
        return new Item(LocaleKeys.Main.i_IonizationUnit,LocaleKeys.Main.id_IonizationUnit)
        {
            Type = ItemType.IonizationUnit,
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
        return new Item(LocaleKeys.Main.i_IonizationUnit, LocaleKeys.Main.id_IonizationUnitP)
        {
            UpgradeCount = 1,
            Type = ItemType.IonizationUnitPlus,
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
        return new Item(LocaleKeys.Main.i_IonizationUnit, LocaleKeys.Main.id_IonizationUnitPP)
        {
            UpgradeCount = 2,
            Type = ItemType.IonizationUnitPlusPlus,
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
        return new Item(LocaleKeys.Main.i_ShiftSystem, LocaleKeys.Main.id_ShiftSystem)
        {
            Unique = true,
            Type = ItemType.ShiftSystem,
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
        return new Item(LocaleKeys.Main.i_DeliveryDevice, LocaleKeys.Main.id_DeliveryDevice)
        {
            Type = ItemType.DeliveryDevice,
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
        return new Item(LocaleKeys.Main.i_DeliveryDevice, LocaleKeys.Main.id_DeliveryDeviceP)
        {
            UpgradeCount = 1,
            Type = ItemType.DeliveryDevicePlus,
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
        return new Item(LocaleKeys.Main.i_DeliveryDevice, LocaleKeys.Main.id_DeliveryDevicePP)
        {
            UpgradeCount = 2,
            Type = ItemType.DeliveryDevicePlusPlus,
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
        return new Item(LocaleKeys.Main.i_Coprocessor)
        {
            Type = ItemType.Coprocessor,
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
        return new Item(LocaleKeys.Main.i_Coprocessor)
        {
            UpgradeCount = 1,
            Type = ItemType.CoprocessorPlus,
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
        return new Item(LocaleKeys.Main.i_Coprocessor)
        {
            UpgradeCount = 2,
            Type = ItemType.CoprocessorPlusPlus,
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
        return new Item(LocaleKeys.Main.i_DivergingLens)
        {
            Type = ItemType.DivergingLens,
            Unique = false,
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
        return new Item(LocaleKeys.Main.i_PowerController)
        {
            Type = ItemType.PowerController,
            NotForSale = true,
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
        return new Item(LocaleKeys.Main.i_PowerController)
        {
            UpgradeCount = 1,
            Type = ItemType.PowerControllerPlus,
            NotForSale = true,
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
        return new Item(LocaleKeys.Main.i_PowerController)
        {
            UpgradeCount = 2,
            Type = ItemType.PowerControllerPlusPlus,
            NotForSale = true,
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
        return new Item(LocaleKeys.Main.i_LensSystem)
        {
            Type = ItemType.LensSystem,
            NotForSale = true,
            Unique = false,
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
        return new Item(LocaleKeys.Main.i_LensSystem)
        {
            UpgradeCount = 1,
            Type = ItemType.LensSystemPlus,
            NotForSale = true,
            Unique = false,
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
        return new Item(LocaleKeys.Main.i_MagneticManipulator)
        {
            Type = ItemType.MagneticManipulator,
            Unique = false,
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
        return new Item(LocaleKeys.Main.i_ReverseSystem, LocaleKeys.Main.id_ReverseSystem)
        {
            Type = ItemType.ReverseSystem,
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
        return new Item(LocaleKeys.Main.i_SpeedDrives)
        {
            Type = ItemType.SpeedDrives,
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
