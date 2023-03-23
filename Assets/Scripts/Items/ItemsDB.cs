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
         ItemType.AtomicBattery
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
            case ItemType.AtomicBattery:
                return AtomicBattery();
            case ItemType.AtomicBatteryPlus:
                return AtomicBatteryPlus();
            case ItemType.AtomicBatteryPlusPlus:
                return AtomicBatteryPlusPlus();
            case ItemType.EnergyAbsorber:
                return EnergyAbsorber();
            case ItemType.Accelerator:
                return Accelerator();
            case ItemType.ArmoredPlates:
                return ArmoredPlates();
            case ItemType.ExoskeletonModule:
                return ExoskeletonModule();
            case ItemType.MagneticCore:
                return MagneticCore();
            case ItemType.OxygenCylinders:
                return OxygenCylinders();
            case ItemType.CoprocessorTwo:
                return CoprocessorTwo();
            case ItemType.FreonCylinders:
                return FreonCylinders();
            case ItemType.AdditionalArmorContour:
                return AdditionalArmorContour();
            case ItemType.StiffeningRibs:
                return StiffeningRibs();
            case ItemType.MovableMechanisms:
                return MovableMechanisms();
            case ItemType.ReinforcedMagneticCore:
                return ReinforcedMagneticCore();
            case ItemType.OxygenSprayer:
                return OxygenSprayer();
            case ItemType.AnalyzingModule:
                return AnalyzingModule();
            case ItemType.FreonSprayer:
                return FreonSprayer();
            case ItemType.OxygenSupplyModule:
                return OxygenSupplyModule();
            case ItemType.PoisonGasCylinders:
                return PoisonGasCylinders();
            case ItemType.None:
            default:
                return Battery();
        }
    }
    public Item Accelerator()
    {
        return new Item(LocaleKeys.Main.i_Accelerator)
        {
            Type = ItemType.Accelerator,
            Cost = 40,
            Icon = Addresses.Ico_Accelerator,
            Perks = new IPerk[] 
            {
                new SimplePerk(PerkType.Speed,
                    new StatModificator(.1f, StatModificatorType.Multiplicative, StatType.MovementSpeed))//+10% скорости
            }
        };
    }
    public Item ArmoredPlates()
    {
        return new Item(LocaleKeys.Main.i_ArmoredPlates)
        {
            Type = ItemType.ArmoredPlates,
            Cost = 40,
            Icon = Addresses.Ico_ArmoredPlates,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerMaxHP,
                    new StatModificator(3f, StatModificatorType.Additive, StatType.MaxHP))//+3 к макс хп.
            }
        };
    }
    public Item ExoskeletonModule()
    {
        return new Item(LocaleKeys.Main.i_ExoskeletonModule)
        {
            Type = ItemType.ExoskeletonModule,
            Cost = 40,
            Icon = Addresses.Ico_ExoskeletonModule,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.Speed,
                    new StatModificator(.5f, StatModificatorType.Additive, StatType.MovementSpeed))//+.5 скорости
            }
        };
    }
    public Item MagneticCore()
    {
        return new Item(LocaleKeys.Main.i_MagneticCore)
        {
            Type = ItemType.MagneticCore,
            Cost = 40,
            Icon = Addresses.Ico_MagneticCore,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.PickupRadius))//+100% радиус подбора
            }
        };
    }
    public Item OxygenCylinders()
    {
        return new Item(LocaleKeys.Main.i_OxygenCylinders)
        {
            Type = ItemType.OxygenCylinders,
            Cost = 40,
            Icon = Addresses.Ico_OxygenCylinders,
            Perks = new IPerk[] 
            {
                new SimplePerk(PerkType.PlayerLifeSupport,
                    new StatModificator(2f, StatModificatorType.Additive, StatType.LifeSupport))//+2 c лайвсапорта
            }
        };
    }
    public Item CoprocessorTwo()
    {
        return new Item(LocaleKeys.Main.i_CoprocessorTwo)
        {
            Type = ItemType.CoprocessorTwo,
            Cost = 40,
            Icon = Addresses.Ico_CoprocessorTwo,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerDodge,
                    new StatModificator(.1f, StatModificatorType.Additive, StatType.Dodge))//+10% а шкансу клониться от урона
            }
        };
    }
    public Item FreonCylinders()
    {
        return new Item(LocaleKeys.Main.i_FreonCylinders)
        {
            Type = ItemType.FreonCylinders,
            Cost = 40,
            Icon = Addresses.Ico_FreonCylinders,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerFireResist,
                    new StatModificator(2f, StatModificatorType.Additive, StatType.FireResist))//+2 защиты от огня
            }
        };          
    }
    public Item AdditionalArmorContour()
    {
        return new Item(LocaleKeys.Main.i_AdditionalArmorContour)
        {
            Type = ItemType.AdditionalArmorContour,
            Cost = 40,
            Icon = Addresses.Ico_AdditionalArmorContour,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerMaxHP,
                    new StatModificator(5f, StatModificatorType.Additive, StatType.MaxHP))//+5 к макс хп
            }
        };
    }
    public Item StiffeningRibs()
    {
        return new Item(LocaleKeys.Main.i_StiffeningRibs)
        {
            Type = ItemType.StiffeningRibs,
            Cost = 40,
            Icon = Addresses.Ico_StiffeningRibs,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerStunResist,
                    new StatModificator(.5f, StatModificatorType.Additive, StatType.StunResist))//+50% защиты от оглушения
            }
        };
    }
    public Item MovableMechanisms()
    {
        return new Item(LocaleKeys.Main.i_MovableMechanisms)
        {
            Type = ItemType.MovableMechanisms,
            Cost = 40,
            Icon = Addresses.Ico_MovableMechanisms,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.Speed,
                    new StatModificator(.8f, StatModificatorType.Additive, StatType.MovementSpeed))//+.8 скорости
            }
        };
    }
    public Item ReinforcedMagneticCore()
    {
        return new Item(LocaleKeys.Main.i_ReinforcedMagneticCore)
        {
            Type = ItemType.ReinforcedMagneticCore,
            Cost = 40,
            Icon = Addresses.Ico_ReinforcedMagneticCore,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.PickupRadius))//+150% радиус подбора
            }
        };
    }
    public Item OxygenSprayer()
    {
        return new Item(LocaleKeys.Main.i_OxygenSprayer)
        {
            Type = ItemType.OxygenSprayer,
            Cost = 40,
            Icon = Addresses.Ico_OxygenSprayer,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerSlowdownResist,
                    new StatModificator(.5f, StatModificatorType.Additive, StatType.SlowdownResist))//+50% защиты от замедления
            }
        };
    }
    public Item AnalyzingModule()
    {
        return new Item(LocaleKeys.Main.i_AnalyzingModule)
        {
            Type = ItemType.AnalyzingModule,
            Cost = 40,
            Icon = Addresses.Ico_AnalyzingModule,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerDodge,
                    new StatModificator(.2f, StatModificatorType.Additive, StatType.Dodge))//+20% а шкансу клониться от урона
            }
        };
    }
    public Item FreonSprayer()
    {
        return new Item(LocaleKeys.Main.i_FreonSprayer)
        {
            Type = ItemType.FreonSprayer,
            Cost = 40,
            Icon = Addresses.Ico_FreonSprayer,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerFireResist,
                    new StatModificator(.3f, StatModificatorType.Additive, StatType.FireResistPercent)),//+30% защиты от огня
                new SimplePerk(PerkType.PlayerElectricityResist,
                    new StatModificator(.3f, StatModificatorType.Additive, StatType.ElectricityResist))//+30% уворота от электричества
            }
        };
    }
    public Item OxygenSupplyModule()
    {
        return new Item(LocaleKeys.Main.i_OxygenSupplyModule)
        {
            Type = ItemType.OxygenSupplyModule,
            Cost = 40,
            Icon = Addresses.Ico_OxygenSupplyModule,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerLifeSupport,
                    new StatModificator(4f, StatModificatorType.Additive, StatType.LifeSupport))//+2 c лайвсапорта
            }
        };
    }
    public Item PoisonGasCylinders()
    {
        return new Item(LocaleKeys.Main.i_PoisonGasCylinders)
        {
            Type = ItemType.PoisonGasCylinders,
            Cost = 40,
            Icon = Addresses.Ico_PoisonGasCylinders,
            Perks = new IPerk[]
            {

            }
        };
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
                new SimplePerk(PerkType.RayPathLenght,
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.Capacity))
            },
        };
    }

    public Item AtomicBattery()
    {
        return new Item(LocaleKeys.Main.i_AtomicBattery)
        {
            Type = ItemType.AtomicBattery,
            Cost = 50,
            Icon = Addresses.Ico_AtomicBattery,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyRegen,
                    new StatModificator(2, StatModificatorType.Additive, StatType.EnergyRegen))
            },
        };
    }
    public Item AtomicBatteryPlus()
    {
        return new Item(LocaleKeys.Main.i_AtomicBattery)
        {
            UpgradeCount = 1,
            Type = ItemType.AtomicBatteryPlus,
            Cost = 100,
            Icon = Addresses.Ico_AtomicBattery,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyRegen,
                    new StatModificator(4, StatModificatorType.Additive, StatType.EnergyRegen))
            },
        };
    }
    public Item AtomicBatteryPlusPlus()
    {
        return new Item(LocaleKeys.Main.i_AtomicBattery)
        {
            UpgradeCount = 1,
            Type = ItemType.AtomicBatteryPlusPlus,
            Cost = 250,
            Icon = Addresses.Ico_AtomicBattery,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyRegen,
                    new StatModificator(8, StatModificatorType.Additive, StatType.EnergyRegen))
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
            Icon = Addresses.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayPathLenght,
                    new StatModificator(.75f, StatModificatorType.Multiplicative, StatType.Capacity))
            },
        };
    }
    public Item BatteryPlusPlus()
    {
        return new Item(LocaleKeys.Main.i_Battery)
        {
            Type = ItemType.BatteryPlusPlus,
            UpgradeCount = 2,
            Cost = 150,
            Icon = Addresses.Ico_Battery,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayPathLenght,
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.Capacity))
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
                new SimplePerk(PerkType.RaySpeed,
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RaySpeed))
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
                new SimplePerk(PerkType.Speed,
                    new StatModificator(0.1f, StatModificatorType.Multiplicative, StatType.MovementSpeed))
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
                new SimplePerk(PerkType.Amplifier,
                    new StatModificator(-0.2f, StatModificatorType.Multiplicative, StatType.Capacity),
                    new StatModificator(0.2f, StatModificatorType.Multiplicative, StatType.RayDamage))
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
                new SimplePerk(PerkType.Amplifier,
                    new StatModificator(-0.3f, StatModificatorType.Multiplicative, StatType.Capacity),
                    new StatModificator(0.4f, StatModificatorType.Multiplicative, StatType.RayDamage))
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
                new SimplePerk(PerkType.Amplifier,
                    new StatModificator(-0.4f, StatModificatorType.Multiplicative, StatType.Capacity),
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamage))
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
                new SimplePerk(PerkType.FocusLens,
                    new StatModificator(-0.9f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius),
                    new StatModificator(0.5f, StatModificatorType.Multiplicative, StatType.RayDamage))
            }
        };
    }
    public Item EnergyAbsorber()
    {
        return new Item(LocaleKeys.Main.i_EnergyAbsorber)
        {
            Type = ItemType.EnergyAbsorber,
            Cost = 60,
            Icon = Addresses.Ico_Laser,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPEnegryAbsorber>(5f)//урон, длительность
            }
        };
    }
    public Item IonizationUnit()
    {
        return new Item(LocaleKeys.Main.i_IonizationUnit, LocaleKeys.Main.id_IonizationUnit)
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
                new SimplePerk(PerkType.RayDelay,
                    new StatModificator(-.3f, StatModificatorType.Multiplicative, StatType.RayDelay),
                    new StatModificator(-1f, StatModificatorType.Additive, StatType.RayError))
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
                new SimplePerk(PerkType.RayDelay, 
                    new StatModificator(-.5f, StatModificatorType.Multiplicative, StatType.RayDelay), 
                    new StatModificator(-2f, StatModificatorType.Additive, StatType.RayError))
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
                new SimplePerk(PerkType.RayDelay, 
                    new StatModificator(-1, StatModificatorType.Multiplicative, StatType.RayDelay), 
                    new StatModificator(-4f, StatModificatorType.Additive, StatType.RayError))
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
                new SimplePerk(PerkType.DivergingLens,
                    new StatModificator(2f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius),
                    new StatModificator(-0.5f, StatModificatorType.Multiplicative, StatType.RayDamage))
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
                new SimplePerk(PerkType.PowerController, 
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.Capacity), 
                    new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayDamage))
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
                new SimplePerk(PerkType.PowerController, 
                    new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.Capacity), 
                    new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.RayDamage))
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
                new SimplePerk(PerkType.PowerController, 
                    new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.Capacity), 
                new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.RayDamage))
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
                new SimplePerk(PerkType.LensSystem,
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))
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
                new SimplePerk(PerkType.LensSystem, 
                    new StatModificator(2f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))
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
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.PickupRadius))
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
                new SimplePerk(PerkType.RayReverse,
                    new StatModificator(1f, StatModificatorType.Additive, StatType.RayReverse),
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed))
            }
        };
    }
    public Item SpeedDrives()
    {
        return new Item(LocaleKeys.Main.i_SpeedDrives)
        {
            UpgradeCount = 1,
            Type = ItemType.SpeedDrives,
            Cost = 100,
            Icon = Addresses.Ico_Servo,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RaySpeed, 
                    new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RaySpeed), 
                    new StatModificator(3, StatModificatorType.Additive, StatType.RayError))
            }
        };
    }
}
