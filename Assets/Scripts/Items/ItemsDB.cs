using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsDB
{
    private ItemType[] shop ={
         //ItemType.AdditionalDrives,
         //ItemType.ExoskeletonSpeedBooster,
         //ItemType.Battery,
         //ItemType.Amplifier,
         //ItemType.FocusLens,
         //ItemType.IonizationUnit,
         //ItemType.DeliveryDevice,
         //ItemType.Coprocessor,
         //ItemType.DivergingLens,
         //ItemType.MagneticManipulator,
         //ItemType.AtomicBattery
         ItemType.Accelerator,
         ItemType.ArmoredPlates,
         ItemType.ExoskeletonModule,
         ItemType.MagneticCore,
         ItemType.OxygenCylinders,
         ItemType.CoprocessorTwo,
         ItemType.FreonCylinders,
         ItemType.DeliveryDeviceTwo,
         ItemType.AtomicCell,
         ItemType.Accumulator,
         ItemType.AdditionalDriveUnits,
         ItemType.AdditionalLens,
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
            case ItemType.AnalyzingModule:
                return AnalyzingModule();
            case ItemType.FreonSprayer:
                return FreonSprayer();
            case ItemType.OxygenSupplyModule:
                return OxygenSupplyModule();
            case ItemType.PoisonGasCylinders:
                return PoisonGasCylinders();
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
            case ItemType.DeliveryDeviceTwo:
                return DeliveryDeviceTwo();
            case ItemType.AtomicCell:
                return AtomicCell();
            case ItemType.Accumulator:
                return Accumulator();
            case ItemType.AdditionalDriveUnits:
                return AdditionalDriveUnits();
            case ItemType.AdditionalLens:
                return AdditionalLens();
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
            case ItemType.ProcessingSlagDropping:
                return ProcessingSlagDropping();
            case ItemType.HighSpeedDriveUnits:
                return HighSpeedDriveUnits();
            case ItemType.DeliveryOfExplosives:
                return DeliveryOfExplosives();
            case ItemType.IonizingPlant:
                return IonizingPlant();
            case ItemType.StableAtomicCell:
                return StableAtomicCell();
            case ItemType.PowerControllerTwo:
                return PowerControllerTwo();
            case ItemType.DiffusionObjective:
                return DiffusionObjective();
            case ItemType.ErrorCalculator:
                return ErrorCalculator();
            case ItemType.FocusingObjective:
                return FocusingObjective();
            case ItemType.DistortionMirror:
                return DistortionMirror();
            case ItemType.PurpleCovering:
                return PurpleCovering();
            case ItemType.ExoskeletonProtection:
                return ExoskeletonProtection();
            case ItemType.ReinforcedCarcass:
                return ReinforcedCarcass();
            case ItemType.ExoskeletonBooster:
                return ExoskeletonBooster();
            case ItemType.MagneticManipulatorTwo:
                return MagneticManipulatorTwo();
            case ItemType.ElectromagneticFieldCore:
                return ElectromagneticFieldCore();
            case ItemType.StarDust:
                return StarDust();
            case ItemType.StarShard:
                return StarShard();
            case ItemType.OxidationSystem:
                return OxidationSystem();
            case ItemType.FireSystem:
                return FireSystem();
            case ItemType.DielectricGasSource:
                return DielectricGasSource();
            case ItemType.ConcentratedPoisonGasCylinders:
                return ConcentratedPoisonGasCylinders();
            case ItemType.OrganicDecompositionDevice:
                return OrganicDecompositionDevice();
            case ItemType.OxygenSaturationModule:
                return OxygenSaturationModule();
            case ItemType.TreatmentSystem:
                return TreatmentSystem();
            case ItemType.Echo:
                return Echo();
            case ItemType.HandmadeFlamethrower:
                return HandmadeFlamethrower();
            case ItemType.WhiteShroud:
                return WhiteShroud();
            case ItemType.IceSoul:
                return IceSoul();
            case ItemType.PoisonousSubstancesDropping:
                return PoisonousSubstancesDropping();
            case ItemType.NonConformingMaterialsDropping:
                return NonConformingMaterialsDropping();
            case ItemType.ExplosiveMixtureDropping:
                return ExplosiveMixtureDropping();
            case ItemType.BakedApplesDelivery:
                return BakedApplesDelivery();
            case ItemType.RadiationContour:
                return RadiationContour();
            case ItemType.IsotopeReactor:
                return IsotopeReactor();
            case ItemType.SmallNuclearReactor:
                return SmallNuclearReactor();
            case ItemType.AtmosphericIonizer:
                return AtmosphericIonizer();
            case ItemType.SelfChargingAccumulator:
                return SelfChargingAccumulator();
            case ItemType.RayOverloadController:
                return RayOverloadController();
            case ItemType.ElectricDriveUnits:
                return ElectricDriveUnits();
            case ItemType.MediumCapacityAccumulator:
                return MediumCapacityAccumulator();
            case ItemType.ErrorCorrector:
                return ErrorCorrector();
            case ItemType.CrystalHalo:
                return CrystalHalo();
            case ItemType.RayDiffusionSystem:
                return RayDiffusionSystem();
            case ItemType.RayFocusingSystem:
                return RayFocusingSystem();
            case ItemType.CrystalLens:
                return CrystalLens();
            case ItemType.FireFootprints:
                return FireFootprints();
            case ItemType.VeilOfDecay:
                return VeilOfDecay();
            case ItemType.WarriorExoskeleton:
                return WarriorExoskeleton();
            case ItemType.SpaceShiftSystem:
                return SpaceShiftSystem();
            case ItemType.PioneerExoskeleton:
                return PioneerExoskeleton();
            case ItemType.GravitationalExplosionDevice:
                return GravitationalExplosionDevice();
            case ItemType.PerpetualMotionMachine:
                return PerpetualMotionMachine();
            case ItemType.StarSatellite:
                return StarSatellite();
            case ItemType.HarvesterExoskeleton:
                return HarvesterExoskeleton();
            case ItemType.EnvironmentalControlSystem:
                return EnvironmentalControlSystem();
            case ItemType.EngineerExoskeleton:
                return EngineerExoskeleton();
            case ItemType.TissueRegenerationSystem:
                return TissueRegenerationSystem();
            case ItemType.CombatFlamethrower:
                return CombatFlamethrower();
            case ItemType.LifeSupportingSystem:
                return LifeSupportingSystem();
            case ItemType.GreatGlaciation:
                return GreatGlaciation();
            case ItemType.TacticExoskeleton:
                return TacticExoskeleton();
            case ItemType.FullUnloading:
                return FullUnloading();
            case ItemType.MeteorRain:
                return MeteorRain();
            case ItemType.Arsenal:
                return Arsenal();
            case ItemType.OrbitalSupportSystem:
                return OrbitalSupportSystem();
            case ItemType.AtomicRay:
                return AtomicRay();
            case ItemType.OverloadedRay:
                return OverloadedRay();
            case ItemType.ChemonuclearReactor:
                return ChemonuclearReactor();
            case ItemType.Lightning:
                return Lightning();
            case ItemType.HighCapacityAccumulator:
                return HighCapacityAccumulator();
            case ItemType.ExtraDriveUnits:
                return ExtraDriveUnits();
            case ItemType.ReverseSystemTwo:
                return ReverseSystemTwo();
            case ItemType.PillarOfLight:
                return PillarOfLight();
            case ItemType.RaySplitter:
                return RaySplitter();
            case ItemType.ArchimedesMirror:
                return ArchimedesMirror();
            case ItemType.FireRay:
                return FireRay();
            case ItemType.Empty:
                return Empty();
            case ItemType.FireGem:
                return FireGem();
            case ItemType.RadiantMineral:
                return RadiantMineral();
            case ItemType.DistortingCrystal:
                return DistortingCrystal();
            case ItemType.ElectricalLead:
                return ElectricalLead();
            case ItemType.GravityStone:
                return GravityStone();
            case ItemType.PoleOfCold:
                return PoleOfCold();
            case ItemType.StoneApple:
                return StoneApple();
            case ItemType.None:
            default:
                return Battery();
        }
    }
    public Item Accelerator()//Акселератор
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
    public Item ArmoredPlates()//Бронированные пластины
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
    public Item ExoskeletonModule()//Модуль экзоскелета
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
    public Item MagneticCore()//Магнитное ядро
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
    public Item OxygenCylinders()//Кислородные баллоны
    {
        return new Item(LocaleKeys.Main.i_OxygenCylinders)
        {
            Type = ItemType.OxygenCylinders,
            Cost = 40,
            Icon = Addresses.Ico_OxygenCylinders,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerLifeSupport,
                               new StatModificator(3f, StatModificatorType.Additive, StatType.LifeSupport))//+3 c лайвсапорта
            }
        };
    }
    public Item CoprocessorTwo()//Сопроцессор
    {
        return new Item(LocaleKeys.Main.i_CoprocessorTwo)
        {
            Type = ItemType.CoprocessorTwo,
            Cost = 40,
            Icon = Addresses.Ico_CoprocessorTwo,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerDodge,
                               new StatModificator(.15f, StatModificatorType.Additive, StatType.Dodge))//+15% а шкансу клониться от урона
            }
        };
    }
    public Item FreonCylinders()//Баллоны хладона
    {
        return new Item(LocaleKeys.Main.i_FreonCylinders)
        {
            Type = ItemType.FreonCylinders,
            Cost = 40,
            Icon = Addresses.Ico_FreonCylinders,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerFireResist,
                               new StatModificator(2f, StatModificatorType.Additive, StatType.FireAbsorption))//+2 защиты от огня
            }
        };
    }
    public Item DeliveryDeviceTwo()//устройство доставки  
    {
        return new Item(LocaleKeys.Main.i_DeliveryDevice)
        {
            Type = ItemType.DeliveryDeviceTwo,
            Cost = 40,
            Icon = Addresses.Ico_DeliveryDevice,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(24f),//раз в 24 с. сбрасывает ящик со взрывчаткой.
            }
        };
    }
    public Item AtomicCell()//Атомная ячейка
    {
        return new Item(LocaleKeys.Main.i_AtomicCell)
        {
            Type = ItemType.AtomicCell,
            Cost = 40,
            Icon = Addresses.Ico_AtomicCell,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyRegen,
                               new StatModificator(2, StatModificatorType.Additive, StatType.EnergyRegen))//генерация энегрии +2 ед.
            }
        };
    }
    public Item Accumulator()//Аккумулятор
    {
        return new Item(LocaleKeys.Main.i_Accumulator)
        {
            Type = ItemType.Accumulator,
            Cost = 40,
            Icon = Addresses.Ico_Accumulator,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyCapacity,
                               new StatModificator(.4f, StatModificatorType.Multiplicative, StatType.Capacity))//базовая емкость энергии +40%.
            }
        };
    }
    public Item AdditionalDriveUnits()//Дополнительные приводы
    {
        return new Item(LocaleKeys.Main.i_AdditionalDriveUnits)
        {
            Type = ItemType.AdditionalDriveUnits,
            Cost = 40,
            Icon = Addresses.Ico_AdditionalDriveUnits,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(.3f, StatModificatorType.Multiplicative, StatType.RaySpeed))//базовая скорость луча +30%.
            }
        };
    }
    public Item AdditionalLens()//Дополнительная линза
    {
        return new Item(LocaleKeys.Main.i_AdditionalLens)
        {
            Type = ItemType.AdditionalLens,
            Cost = 40,
            Icon = Addresses.Ico_AdditionalLens,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.2f, StatModificatorType.Multiplicative, StatType.RayDamage))//Базовый урон луча +20%.
            }
        };
    }
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////желтые арты
    /// </summary>
    /// <returns></returns>
    public Item AdditionalArmorContour()//Дополнительный контур брони
    {
        return new Item(LocaleKeys.Main.i_AdditionalArmorContour)
        {
            Type = ItemType.AdditionalArmorContour,
            Cost = 80,
            Icon = Addresses.Ico_AdditionalArmorContour,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerMaxHP,
                    new StatModificator(5f, StatModificatorType.Additive, StatType.MaxHP))//+5 к макс хп
            }
        };
    }
    public Item StiffeningRibs()//Ребра жесткости
    {
        return new Item(LocaleKeys.Main.i_StiffeningRibs)
        {
            Type = ItemType.StiffeningRibs,
            Cost = 80,
            Icon = Addresses.Ico_StiffeningRibs,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerStunResist,
                    new StatModificator(.5f, StatModificatorType.Additive, StatType.StunResist))//+50% защиты от оглушения
            }
        };
    }
    public Item MovableMechanisms()//Подвижные механизмы
    {
        return new Item(LocaleKeys.Main.i_MovableMechanisms)
        {
            Type = ItemType.MovableMechanisms,
            Cost = 80,
            Icon = Addresses.Ico_MovableMechanisms,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.Speed,
                    new StatModificator(.8f, StatModificatorType.Additive, StatType.MovementSpeed))//+.8 скорости
            }
        };
    }
    public Item ReinforcedMagneticCore()//Усиленное магнитное ядро
    {
        return new Item(LocaleKeys.Main.i_ReinforcedMagneticCore)
        {
            Type = ItemType.ReinforcedMagneticCore,
            Cost = 80,
            Icon = Addresses.Ico_ReinforcedMagneticCore,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.PickupRadius))//+150% радиус подбора
            }
        };
    }
    public Item OxygenSprayer()//Кислородный распылитель
    {
        return new Item(LocaleKeys.Main.i_OxygenSprayer)
        {
            Type = ItemType.OxygenSprayer,
            Cost = 80,
            Icon = Addresses.Ico_OxygenSprayer,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerSlowdownResist,
                    new StatModificator(.5f, StatModificatorType.Additive, StatType.SlowdownResist))//+50% защиты от замедления
            }
        };
    }
    public Item AnalyzingModule()//Анализирующий модуль
    {
        return new Item(LocaleKeys.Main.i_AnalyzingModule)
        {
            Type = ItemType.AnalyzingModule,
            Cost = 80,
            Icon = Addresses.Ico_AnalyzingModule,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerDodge,
                    new StatModificator(.2f, StatModificatorType.Additive, StatType.Dodge))//+20% а шкансу клониться от урона
            }
        };
    }
    public Item FreonSprayer()//Распылитель хладона
    {
        return new Item(LocaleKeys.Main.i_FreonSprayer)
        {
            Type = ItemType.FreonSprayer,
            Cost = 80,
            Icon = Addresses.Ico_FreonSprayer,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerFireResist,
                    new StatModificator(.3f, StatModificatorType.Additive, StatType.FireResist)),//+30% защиты от огня
                new SimplePerk(PerkType.PlayerElectricityResist,
                    new StatModificator(.3f, StatModificatorType.Additive, StatType.ElectricityResist))//+30% уворота от электричества
            }
        };
    }
    public Item OxygenSupplyModule()//Модуль подачи кислорода
    {
        return new Item(LocaleKeys.Main.i_OxygenSupplyModule)
        {
            Type = ItemType.OxygenSupplyModule,
            Cost = 80,
            Icon = Addresses.Ico_OxygenSupplyModule,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerLifeSupport,
                    new StatModificator(4f, StatModificatorType.Additive, StatType.LifeSupport))//+4 c лайвсапорта
            }
        };
    }
    public Item PoisonGasCylinders()//Балоны отравляющего газа
    {
        return new Item(LocaleKeys.Main.i_PoisonGasCylinders)
        {
            Type = ItemType.PoisonGasCylinders,
            Cost = 80,
            Icon = Addresses.Ico_PoisonGasCylinders,
            Perks = new IPerk[]
            {
                //Оставляет за собой дымку [[Ядовитый газ]].
            }
        };
    }
    public Item ProcessingSlagDropping()//Сброс шлаков переработки
    {
        return new Item(LocaleKeys.Main.i_ProcessingSlagDropping)
        {
            Type = ItemType.ProcessingSlagDropping,
            Cost = 80,
            Icon = Addresses.Ico_ProcessingSlagDropping,
            Perks = new IPerk[]
            {
                //Раз в 16 с. в случайную точку [[Зона контроля]] падает снаряд, наносящий 60 ед.
                //[[Физический урон]] в центре, и 20 ед. по бокам.
                //Радиус поражения - 10 п.
            }
        };
    }
    public Item HighSpeedDriveUnits()//Скоростные приводы
    {
        return new Item(LocaleKeys.Main.i_HighSpeedDriveUnits)
        {
            Type = ItemType.HighSpeedDriveUnits,
            Cost = 80,
            Icon = Addresses.Ico_HighSpeedDriveUnits,
            Perks = new IPerk[]
            {
               new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RayError)),//[[Базовая скорость луча]] +50 %.                             
               new SimplePerk(PerkType.RayError,
                              new StatModificator(1, StatModificatorType.Additive, StatType.RayError))//Погрешность + 1п.)
            }
        };
    }
    public Item DeliveryOfExplosives()//Доставка взрывоопасных веществ
    {
        return new Item(LocaleKeys.Main.i_DeliveryOfExplosives)
        {
            Type = ItemType.DeliveryOfExplosives,
            Cost = 80,
            Icon = Addresses.Ico_DeliveryOfExplosives,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(16f),//cooldown//Раз в 16 с. сбрасывает [[Ящик со взрывчаткой]].
            }
        };
    }
    public Item IonizingPlant()//Ионизирующая установка
    {
        return new Item(LocaleKeys.Main.i_IonizingPlant)
        {
            Type = ItemType.IonizingPlant,
            Cost = 80,
            Icon = Addresses.Ico_IonizingPlant,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>(4f, 4f)//Пораженные лучем враги получают статус [[Радиация]] с уроном в 4 единицы.
            }
        };
    }
    public Item StableAtomicCell()//Стабильная атомная ячейка
    {
        return new Item(LocaleKeys.Main.i_StableAtomicCell)
        {
            Type = ItemType.StableAtomicCell,
            Cost = 80,
            Icon = Addresses.Ico_StableAtomicCell,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyRegen,
                               new StatModificator(3, StatModificatorType.Additive, StatType.EnergyRegen))//[[Генерация энергии]] +3 ед./с.
            }
        };
    }
    public Item AccumulatorBattery()//Аккумуляторная батарея
    {
        return new Item(LocaleKeys.Main.i_AccumulatorBattery)
        {
            Type = ItemType.AccumulatorBattery,
            Cost = 80,
            Icon = Addresses.Ico_AccumulatorBattery,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyCapacity,
                               new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.Capacity))//[[Базовая ёмкость энергии]] + 60%.
            }
        };
    }
    public Item PowerControllerTwo()//Контроллер питания
    {
        return new Item(LocaleKeys.Main.i_PowerControllerTwo)
        {
            Type = ItemType.PowerControllerTwo,
            Cost = 80,
            Icon = Addresses.Ico_PowerControllerTwo,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayCostReduction,
                               new StatModificator(1, StatModificatorType.Additive, StatType.RayCostReduction))//Уменьшает стоимсоть луча за пройденную длину луча.
                
            }
        };
    }
    public Item DiffusionObjective()//рассеивающий объектив
    {
        return new Item(LocaleKeys.Main.i_DiffusionObjective)
        {
            Type = ItemType.DiffusionObjective,
            Cost = 80,
            Icon = Addresses.Ico_DiffusionObjective,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(-.4f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] -40 %.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(0.8f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))//[[Радиус поражения луча]] +80 %
            }
        };
    }
    public Item ErrorCalculator()//Вычислитель погрешностей
    {
        return new Item(LocaleKeys.Main.i_ErrorCalculator)
        {
            Type = ItemType.ErrorCalculator,
            Cost = 80,
            Icon = Addresses.Ico_ErrorCalculator,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayError,
                              new StatModificator(-3, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] уменьшается на -3.
            }
        };
    }
    public Item FocusingObjective()//Фокусирующий объектив
    {
        return new Item(LocaleKeys.Main.i_FocusingObjective)
        {
            Type = ItemType.FocusingObjective,
            Cost = 80,
            Icon = Addresses.Ico_FocusingObjective,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.4f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +40 %.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(-0.4f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)),//[[Радиус поражения луча]] -40 %.
                new SimplePerk(PerkType.RayError,
                              new StatModificator(1, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]]Погрешность + 1п.
            }
        };
    }
    public Item PlasmaSupercharger()//Нагнетатель плазмы
    {
        return new Item(LocaleKeys.Main.i_PlasmaSupercharger)
        {
            Type = ItemType.PlasmaSupercharger,
            Cost = 80,
            Icon = Addresses.Ico_PlasmaSupercharger,
            Perks = new IPerk[]
            {
                //Накапливает заряд, следующий луч оставляет огненный след.
            }
        };
    }
    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////оранживые арты
    /// </summary>
    /// <returns></returns>
    public Item DistortionMirror()//Зеркало искажения
    {
        return new Item(LocaleKeys.Main.i_DistortionMirror)
        {
            Type = ItemType.DistortionMirror,
            Cost = 160,
            Icon = Addresses.Ico_DistortionMirror,
            Perks = new IPerk[]
            {
                //_perksFactory.Create<PerkPShiftSystem>(),//После получаения урона персонаж получает статус [[Имматериальный]] на 3 с.
                //Наносимый персонажу урон увеличивается на 1.
            }
        };
    }
    public Item PurpleCovering()//Фиолетовый покров
    {
        return new Item(LocaleKeys.Main.i_PurpleCovering)
        {
            Type = ItemType.PurpleCovering,
            Cost = 160,
            Icon = Addresses.Ico_PurpleCovering,
            Perks = new IPerk[]
            {
                //При получении урона выпускает волну [[Ядовитый газ]] радиусом 20 п.
                new SimplePerk(PerkType.PlayerMaxHP,
                               new StatModificator(5f, StatModificatorType.Additive, StatType.MaxHP))//[[Мак хит поинтов]] +5 ед.
            }
        };
    }
    public Item ExoskeletonProtection()//Защита экзоскелета
    {
        return new Item(LocaleKeys.Main.i_ExoskeletonProtection)
        {
            Type = ItemType.ExoskeletonProtection,
            Cost = 160,
            Icon = Addresses.Ico_ExoskeletonProtection,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerMaxHP,
                    new StatModificator(10f, StatModificatorType.Additive, StatType.MaxHP))//[[Мак хит поинтов]] + 10 ед.
            }
        };
    }
    public Item ReinforcedCarcass()//Усиленный каркас
    {
        return new Item(LocaleKeys.Main.i_ReinforcedCarcass)
        {
            Type = ItemType.ReinforcedCarcass,
            Cost = 160,
            Icon = Addresses.Ico_ReinforcedCarcass,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerStunResist,
                    new StatModificator(1f, StatModificatorType.Additive, StatType.StunResist))//[[Оглушение]] не действует на персонажа.
            }
        };
    }
    public Item ExoskeletonBooster()//Ускоритель экзоскелета
    {
        return new Item(LocaleKeys.Main.i_ExoskeletonBooster)
        {
            Type = ItemType.ExoskeletonBooster,
            Cost = 160,
            Icon = Addresses.Ico_ExoskeletonBooster,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.Speed,
                               new StatModificator(1.6f, StatModificatorType.Additive, StatType.MovementSpeed))//[[Скорость передвижения]] +1.6 п/с.
            }
        };
    }
    public Item MagneticManipulatorTwo()//Магнитный манипулятор
    {
        return new Item(LocaleKeys.Main.i_MagneticManipulatorTwo)
        {
            Type = ItemType.MagneticManipulatorTwo,
            Cost = 160,
            Icon = Addresses.Ico_MagneticManipulatorTwo,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(3f, StatModificatorType.Multiplicative, StatType.PickupRadius))//[[Радиус сбора]] + 300%.
            }
        };
    }
    public Item ElectromagneticFieldCore()//Ядро электромагнитного поля
    {
        return new Item(LocaleKeys.Main.i_ElectromagneticFieldCore)
        {
            Type = ItemType.ElectromagneticFieldCore,
            Cost = 160,
            Icon = Addresses.Ico_ElectromagneticFieldCore,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerRadiationResist,
                    new StatModificator(.9f, StatModificatorType.Additive, StatType.RadiationResist))//Уменьшает на 90% урон от [[Радиация]].
            }
        };
    }
    public Item StarDust()//Звездная пыль
    {
        return new Item(LocaleKeys.Main.i_StarDust)
        {
            Type = ItemType.StarDust,
            Cost = 160,
            Icon = Addresses.Ico_StarDust,
            Perks = new IPerk[]
            {
                new PerkStarDust(10, 2, 5, 0, .5f, 10, 6, PerkType.GravityMatter),
                //При поднятии руды получаете малый осколок материи, который  вращается вокруг персонажа в радиусе 30 п., наносящий 5 ед.
                //[[Физический урон]] в течение 20 с.
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(1.5f, StatModificatorType.Multiplicative, StatType.PickupRadius))//[[Радиус сбора]] +150 %.
            }
        };
    }
    public Item StarShard()//Звездный осколок
    {
        return new Item(LocaleKeys.Main.i_StarShard)
        {
            Type = ItemType.StarShard,
            Cost = 160,
            Icon = Addresses.Ico_StarShard,
            Perks = new IPerk[]
            {
                new PerkGravityMatter(20, 1.5f, 20, 1, 1, PerkType.GravityMatter)//Вращает осколок материи вокруг персонажа в радиусе 15 п.
                                                                           //Осколок наносит 20 ед.  [[Физический урон]] и оглушает на 1 секунды.
            }
        };
    }
    public Item OxidationSystem()//Система окисления
    {
        return new Item(LocaleKeys.Main.i_OxidationSystem)
        {
            Type = ItemType.OxidationSystem,
            Cost = 160,
            Icon = Addresses.Ico_OxidationSystem,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerSlowdownResist,
                    new StatModificator(1f, StatModificatorType.Additive, StatType.SlowdownResist))//Жижа не замедляет.
                //Жижа  растворяется.
            }
        };
    }
    public Item FireSystem()//Пожарная система
    {
        return new Item(LocaleKeys.Main.i_FireSystem)
        {
            Type = ItemType.FireSystem,
            Cost = 160,
            Icon = Addresses.Ico_FireSystem,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerFireResist,
                    new StatModificator(.8f, StatModificatorType.Additive, StatType.FireResist))//[[Огонь]] наносит вам на 80% урона меньше.
            }
        };
    }
    public Item DielectricGasSource()//Источник элегаза
    {
        return new Item(LocaleKeys.Main.i_DielectricGasSource)
        {
            Type = ItemType.DielectricGasSource,
            Cost = 160,
            Icon = Addresses.Ico_DielectricGasSource,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerElectricityResist,
                    new StatModificator(.8f, StatModificatorType.Additive, StatType.ElectricityResist))//Дает 80% шанс уклониться от воздействия [[Электричество]].
            }
        };
    }
    public Item ConcentratedPoisonGasCylinders()//Балоны концентрированного отравляющего газа
    {
        return new Item(LocaleKeys.Main.i_ConcentratedPoisonGasCylinders)
        {
            Type = ItemType.ConcentratedPoisonGasCylinders,
            Cost = 160,
            Icon = Addresses.Ico_ConcentratedPoisonGasCylinders,
            Perks = new IPerk[]
            {
                //Оставляет за собой  [[Ядовитый газ]].
            }
        };
    }
    public Item OrganicDecompositionDevice()//Устройство органического разложения
    {
        return new Item(LocaleKeys.Main.i_OrganicDecompositionDevice)
        {
            Type = ItemType.OrganicDecompositionDevice,
            Cost = 160,
            Icon = Addresses.Ico_OrganicDecompositionDevice,
            Perks = new IPerk[]
            {
                //Врывает ядовитым облаком все подобранные хилки.
            }
        };
    }
    public Item OxygenSaturationModule()//Модуль насыщения кислородом
    {
        return new Item(LocaleKeys.Main.i_OxygenSaturationModule)
        {
            Type = ItemType.OxygenSaturationModule,
            Cost = 160,
            Icon = Addresses.Ico_OxygenSaturationModule,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerLifeSupport,
                               new StatModificator(8f, StatModificatorType.Additive, StatType.LifeSupport))//[[Запас кислорода]] + 8 с.
            }
        };
    }
    public Item TreatmentSystem()//Система лечения
    {
        return new Item(LocaleKeys.Main.i_TreatmentSystem)
        {
            Type = ItemType.TreatmentSystem,
            Cost = 160,
            Icon = Addresses.Ico_TreatmentSystem,
            Perks = new IPerk[]
            {
                //[[Востановление хп]]  +1 раз в 16 с.
            }
        };
    }
    public Item Echo()//Эхо
    {
        return new Item(LocaleKeys.Main.i_Echo)
        {
            Type = ItemType.Echo,
            Cost = 160,
            Icon = Addresses.Ico_Echo,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.MineralEffect,
                               new StatModificator(1f, StatModificatorType.Additive, StatType.MineralEffect))//Удваевает эффекты минералов в ващем интвентаре.
            }
        };
    }
    public Item HandmadeFlamethrower()//Самодельный огнемет
    {
        return new Item(LocaleKeys.Main.i_HandmadeFlamethrower)
        {
            Type = ItemType.HandmadeFlamethrower,
            Cost = 160,
            Icon = Addresses.Ico_HandmadeFlamethrower,
            Perks = new IPerk[]
            {

            }
        };
    }
    public Item WhiteShroud()//Белый саван
    {
        return new Item(LocaleKeys.Main.i_WhiteShroud)
        {
            Type = ItemType.WhiteShroud,
            Cost = 40,
            Icon = Addresses.Ico_WhiteShroud,
            Perks = new IPerk[]
            {
                new PerkColdAOE(15),//При получении урона по персонажу выпускает волну [[Обморожение]] радиусом 20 п.
                new SimplePerk(PerkType.PlayerFireResist,
                    new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.FireResist))//[[Огонь]] наносит персонажу на 50 % урона меньше.
            }
        };
    }
    public Item IceSoul()//Ледяная душа
    {
        return new Item(LocaleKeys.Main.i_IceSoul)
        {
            Type = ItemType.IceSoul,
            Cost = 160,
            Icon = Addresses.Ico_IceSoul,
            Perks = new IPerk[]
            {
                //Накладывает [[Обморожение]] на 10 врагов в радиусе 40 п. раз в 10 с.
            }
        };
    }
    public Item PoisonousSubstancesDropping()//Сброс ядовитых веществ
    {
        return new Item(LocaleKeys.Main.i_PoisonousSubstancesDropping)
        {
            Type = ItemType.PoisonousSubstancesDropping,
            Cost = 160,
            Icon = Addresses.Ico_PoisonousSubstancesDropping,
            Perks = new IPerk[]
            {
                //Раз в 8 с. в случайную точку [[Зона контроля]] падает снаряд, выпускает волну [[Ядовитый газ]] радиусом 15 п.
            }
        };
    }
    public Item NonConformingMaterialsDropping()//Сброс некондиционных материалов
    {
        return new Item(LocaleKeys.Main.i_NonConformingMaterialsDropping)
        {
            Type = ItemType.NonConformingMaterialsDropping,
            Cost = 160,
            Icon = Addresses.Ico_NonConformingMaterialsDropping,
            Perks = new IPerk[]
            {
                //Раз в 8 с. в случайную точку [[Зона контроля]] падает снаряд, наносящий 60 ед.
                //[[Физический урон]] в центре, и 20 ед. по бокам.
                //Радиус поражения - 10 п.
            }
        };
    }
    public Item ExplosiveMixtureDropping()//Доставка взрывоопасной смеси
    {
        return new Item(LocaleKeys.Main.i_ExplosiveMixtureDropping)
        {
            Type = ItemType.ExplosiveMixtureDropping,
            Cost = 160,
            Icon = Addresses.Ico_ExplosiveMixtureDropping,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPBarrels>(8f),//Раз в 8 с. сбрасывает [[Ящик со взрывчаткой]].
            }
        };
    }
    public Item BakedApplesDelivery()//Доставка печеных яблок
    {
        return new Item(LocaleKeys.Main.i_BakedApplesDelivery)
        {
            Type = ItemType.BakedApplesDelivery,
            Cost = 160,
            Icon = Addresses.Ico_BakedApplesDelivery,
            Perks = new IPerk[]
            {
                //Раз в 14 с. сбрасывает [[Ящик со взрывчаткой]].
                //После взрыва оставляет после себя хилку.
                //[[Востановление хп]] 1 ед.
            }
        };
    }
    public Item RadiationContour()//Радиационный контур
    {
        return new Item(LocaleKeys.Main.i_RadiationContour)
        {
            Type = ItemType.RadiationContour,
            Cost = 160,
            Icon = Addresses.Ico_RadiationContour,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkPIonization>(8f, 4f)//Пораженные лучем враги получают статус [[Радиация]] с уроном в 8 единицы.

            }
        };
    }
    public Item IsotopeReactor()//Изотопный реактор
    {
        return new Item(LocaleKeys.Main.i_IsotopeReactor)
        {
            Type = ItemType.IsotopeReactor,
            Cost = 160,
            Icon = Addresses.Ico_IsotopeReactor,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyRegen,
                               new StatModificator(6, StatModificatorType.Additive, StatType.EnergyRegen))//[[Генерация энергии]] +6 ед./с.

            }
        };
    }
    public Item SmallNuclearReactor()//Атомный реактор малой мощьности
    {
        return new Item(LocaleKeys.Main.i_SmallNuclearReactor)
        {
            Type = ItemType.SmallNuclearReactor,
            Cost = 160,
            Icon = Addresses.Ico_SmallNuclearReactor,
            Perks = new IPerk[]
            {
                //Генерация 5% от максимального заряда энергии в сек.

            }
        };
    }
    public Item AtmosphericIonizer()//Ионизатор атмосферы
    {
        return new Item(LocaleKeys.Main.i_AtmosphericIonizer)
        {
            Type = ItemType.AtmosphericIonizer,
            Cost = 160,
            Icon = Addresses.Ico_AtmosphericIonizer,
            Perks = new IPerk[]
            {
                //С каждой пройденым п. увеличивает появление молнии.

            }
        };
    }
    public Item SelfChargingAccumulator()//Самозарядный аккумулятор
    {
        return new Item(LocaleKeys.Main.i_SelfChargingAccumulator)
        {
            Type = ItemType.SelfChargingAccumulator,
            Cost = 160,
            Icon = Addresses.Ico_SelfChargingAccumulator,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyCapacity,
                               new StatModificator(.6f, StatModificatorType.Multiplicative, StatType.Capacity)),//[[Базовая ёмкость энергии]] + 60%.
                new SimplePerk(PerkType.EnergyRegen,
                               new StatModificator(3, StatModificatorType.Additive, StatType.EnergyRegen))//[[Генерация энергии]] +3 ед./ с.

            }
        };
    }
    public Item RayOverloadController()//Контроллер перегрузки луча
    {
        return new Item(LocaleKeys.Main.i_RayOverloadController)
        {
            Type = ItemType.RayOverloadController,
            Cost = 160,
            Icon = Addresses.Ico_RayOverloadController,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.5f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +50%.
                new SimplePerk(PerkType.RayDelay,
                               new StatModificator(1f, StatModificatorType.Additive, StatType.RayDelay))//[[Задержка активации луча]] +1 секунды.

            }
        };
    }
    public Item ElectricDriveUnits()//Электроприводы
    {
        return new Item(LocaleKeys.Main.i_ElectricDriveUnits)
        {
            Type = ItemType.ElectricDriveUnits,
            Cost = 160,
            Icon = Addresses.Ico_ElectricDriveUnits,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(1, StatModificatorType.Additive, StatType.RaySpeed)),//[[Базовая скорость луча]] +100%.
                new SimplePerk(PerkType.RayError,
                              new StatModificator(2, StatModificatorType.Additive, StatType.RayError))//Погрешность +2п.
            }
        };
    }
    public Item MediumCapacityAccumulator()//Аккумулятор средней ёмкости
    {
        return new Item(LocaleKeys.Main.i_MediumCapacityAccumulator)
        {
            Type = ItemType.MediumCapacityAccumulator,
            Cost = 160,
            Icon = Addresses.Ico_MediumCapacityAccumulator,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.EnergyCapacity,
                               new StatModificator(1.2f, StatModificatorType.Multiplicative, StatType.Capacity))//[[Базовая ёмкость энергии]] + 120%.
            }
        };
    }
    public Item ErrorCorrector()//Корректор погрешностей
    {
        return new Item(LocaleKeys.Main.i_ErrorCorrector)
        {
            Type = ItemType.ErrorCorrector,
            Cost = 160,
            Icon = Addresses.Ico_ErrorCorrector,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayError,
                              new StatModificator(-3, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] уменьшается на -3.
                //Уменьшает стоимсоть луча за пройденную длину луча.

            }
        };
    }
    public Item CrystalHalo()//Хрустальное хало
    {
        return new Item(LocaleKeys.Main.i_CrystalHalo)
        {
            Type = ItemType.CrystalHalo,
            Cost = 160,
            Icon = Addresses.Ico_CrystalHalo,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(-.8f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] - 80%.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(1.6f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))//[[Радиус поражения луча]] +160 %.

            }
        };
    }
    public Item RayDiffusionSystem()//Система рассеивания луча
    {
        return new Item(LocaleKeys.Main.i_RayDiffusionSystem)
        {
            Type = ItemType.RayDiffusionSystem,
            Cost = 160,
            Icon = Addresses.Ico_RayDiffusionSystem,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(0.8f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))//[[Радиус поражения луча]] + 80%.
            }
        };
    }
    public Item RayFocusingSystem()//Система фокусировки луча
    {
        return new Item(LocaleKeys.Main.i_RayFocusingSystem)
        {
            Type = ItemType.RayFocusingSystem,
            Cost = 160,
            Icon = Addresses.Ico_RayFocusingSystem,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.4f, StatModificatorType.Multiplicative, StatType.RayDamage))//[[Базовый урон луча]] +40%.

            }
        };
    }
    public Item CrystalLens()//Хрустальная линза
    {
        return new Item(LocaleKeys.Main.i_CrystalLens)
        {
            Type = ItemType.CrystalLens,
            Cost = 160,
            Icon = Addresses.Ico_CrystalLens,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(.8f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +80%.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(-0.8f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)),//[[Радиус поражения луча]] -80 %.
                new SimplePerk(PerkType.RayError,
                              new StatModificator(1, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] +1п.

            }
        };
    }
    public Item FireFootprints()//Огненные следы
    {
        return new Item(LocaleKeys.Main.i_FireFootprints)
        {
            Type = ItemType.FireFootprints,
            Cost = 160,
            Icon = Addresses.Ico_FireFootprints,
            Perks = new IPerk[]
            {
                //Накапливает заряд, следующий луч оставляет огненный след.


            }
        };
    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////красные арты
    /// </summary>
    /// <returns></returns>
    public Item VeilOfDecay()//Покров разложения
    {
        return new Item(LocaleKeys.Main.i_VeilOfDecay)
        {
            Type = ItemType.VeilOfDecay,
            Cost = 320,
            Icon = Addresses.Ico_VeilOfDecay,
            Perks = new IPerk[]
            {
                //Врывает ядовитым облаком все подобранные хилки.
                //При получении урона выпускает волну [[Ядовитый газ]] радиусом 20 п.
                new SimplePerk(PerkType.PlayerMaxHP,
                               new StatModificator(5f, StatModificatorType.Additive, StatType.MaxHP))//[[Мак хит поинтов]] + 5 ед.


            }
        };
    }
    public Item WarriorExoskeleton()//Экзоскелет война
    {
        return new Item(LocaleKeys.Main.i_WarriorExoskeleton)
        {
            Type = ItemType.WarriorExoskeleton,
            Cost = 320,
            Icon = Addresses.Ico_WarriorExoskeleton,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerStunResist,
                               new StatModificator(1f, StatModificatorType.Additive, StatType.StunResist)),//[[Оглушение]] не действует на персонажа.
                new SimplePerk(PerkType.PlayerMaxHP,
                               new StatModificator(10f, StatModificatorType.Additive, StatType.MaxHP))//Макс хп + 10.
                //[[Экзоскелет]].



            }
        };
    }
    public Item SpaceShiftSystem()//Система сдвига пространства
    {
        return new Item(LocaleKeys.Main.i_SpaceShiftSystem)
        {
            Type = ItemType.SpaceShiftSystem,
            Cost = 320,
            Icon = Addresses.Ico_SpaceShiftSystem,
            Perks = new IPerk[]
            {
                //После получаения урона персонаж получает статус [[Имматериальный]] на 3 с.
                //Наносимый персонажу урон увеличивается на 1.
                new SimplePerk(PerkType.Speed,
                               new StatModificator(1.6f, StatModificatorType.Additive, StatType.MovementSpeed))//[[Скорость передвижения]] +1.6 п/с.



            }
        };
    }
    public Item PioneerExoskeleton()//Экзоскелет пионера
    {
        return new Item(LocaleKeys.Main.i_PioneerExoskeleton)
        {
            Type = ItemType.PioneerExoskeleton,
            Cost = 320,
            Icon = Addresses.Ico_PioneerExoskeleton,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.Speed,
                               new StatModificator(1.6f, StatModificatorType.Additive, StatType.MovementSpeed)),//[[Скорость передвижения]] +1.6 п/с.
                new SimplePerk(PerkType.PlayerLifeSupport,
                               new StatModificator(8f, StatModificatorType.Additive, StatType.LifeSupport))//[[Запас кислорода]] +8 с.
                //[[Экзоскелет]].




            }
        };
    }
    public Item GravitationalExplosionDevice()//Устройство гравитационного взрыва материи
    {
        return new Item(LocaleKeys.Main.i_GravitationalExplosionDevice)
        {
            Type = ItemType.GravitationalExplosionDevice,
            Cost = 320,
            Icon = Addresses.Ico_GravitationalExplosionDevice,
            Perks = new IPerk[]
            {
                //Подберая руду, она взрывается на 8 осколков, летящих на 30 п. и наносяших 40 ед.  [[Физический урон]].



            }
        };
    }
    public Item PerpetualMotionMachine()//Вечный двигатель
    {
        return new Item(LocaleKeys.Main.i_PerpetualMotionMachine)
        {
            Type = ItemType.PerpetualMotionMachine,
            Cost = 320,
            Icon = Addresses.Ico_PerpetualMotionMachine,
            Perks = new IPerk[]
            {
                //Раз в 3 с. выпускает в случайного врага молнию, бьющую 20 ед. [[Электричество]].



            }
        };
    }
    public Item StarSatellite()//Звездный спутник
    {
        return new Item(LocaleKeys.Main.i_StarSatellite)
        {
            Type = ItemType.StarSatellite,
            Cost = 320,
            Icon = Addresses.Ico_StarSatellite,
            Perks = new IPerk[]
            {
                new PerkGravityMatter(25, 1, 40, 2, 2, PerkType.GravityMatter)//Вращает осколок материи вокруг персонажа в радиусе 25 п.
                                                                           //Осколок наносит 40 ед.  [[Физический урон]] и оглушает на 2 секунды.



            }
        };
    }
    public Item HarvesterExoskeleton()//Экзоскелет харвестера
    {
        return new Item(LocaleKeys.Main.i_HarvesterExoskeleton)
        {
            Type = ItemType.HarvesterExoskeleton,
            Cost = 320,
            Icon = Addresses.Ico_HarvesterExoskeleton,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.MagneticManipulator,
                    new StatModificator(3f, StatModificatorType.Multiplicative, StatType.PickupRadius)),//[[Радиус сбора]] + 300%.
                new SimplePerk(PerkType.PlayerSlowdownResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.SlowdownResist))//Жижа не замедляет
                //Жижа растворяется
                //[[Экзоскелет]].



            }
        };
    }
    public Item EnvironmentalControlSystem()//Система контроля окружающей среды
    {
        return new Item(LocaleKeys.Main.i_EnvironmentalControlSystem)
        {
            Type = ItemType.EnvironmentalControlSystem,
            Cost = 320,
            Icon = Addresses.Ico_EnvironmentalControlSystem,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerSlowdownResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.SlowdownResist)),//Жижа не замедляет
                //Жижа растворяется

                new SimplePerk(PerkType.PlayerFireResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.FireAbsorption))//[[Огонь]] не наносит урона.


               
            }
        };
    }
    public Item EngineerExoskeleton()//Экзоскелет инженера
    {
        return new Item(LocaleKeys.Main.i_EngineerExoskeleton)
        {
            Type = ItemType.EngineerExoskeleton,
            Cost = 320,
            Icon = Addresses.Ico_EngineerExoskeleton,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.PlayerElectricityResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.ElectricityResist)),//[[Электричество]] не воздействует на персонажа
                new SimplePerk(PerkType.PlayerRadiationResist,
                               new StatModificator(1, StatModificatorType.Additive, StatType.RadiationResist))//[[Радиация]] не воздействует на персонажа.
                //[[Экзоскелет]].


            }
        };
    }
    public Item TissueRegenerationSystem()//Система регенерации тканей
    {
        return new Item(LocaleKeys.Main.i_TissueRegenerationSystem)
        {
            Type = ItemType.TissueRegenerationSystem,
            Cost = 40,
            Icon = Addresses.Ico_TissueRegenerationSystem,
            Perks = new IPerk[]
            {
                //[[Востановление хп]]  +1 раз в 8 с.



            }
        };
    }
    public Item CombatFlamethrower()//Боевой огнемет
    {
        return new Item(LocaleKeys.Main.i_CombatFlamethrower)
        {
            Type = ItemType.CombatFlamethrower,
            Cost = 320,
            Icon = Addresses.Ico_CombatFlamethrower,
            Perks = new IPerk[]
            {




            }
        };
    }
    public Item LifeSupportingSystem()//Система жизнеобеспечения
    {
        return new Item(LocaleKeys.Main.i_LifeSupportingSystem)
        {
            Type = ItemType.LifeSupportingSystem,
            Cost = 320,
            Icon = Addresses.Ico_LifeSupportingSystem,
            Perks = new IPerk[]
            {

                new SimplePerk(PerkType.PlayerLifeSupport,
                               new StatModificator(8f, StatModificatorType.Additive, StatType.LifeSupport))//[[Запас кислорода]] + 8 с.
                //[[Востановление хп]]  +1 раз в 16 с.


            }
        };
    }
    public Item GreatGlaciation()//Великое оледенение
    {
        return new Item(LocaleKeys.Main.i_GreatGlaciation)
        {
            Type = ItemType.GreatGlaciation,
            Cost = 320,
            Icon = Addresses.Ico_GreatGlaciation,
            Perks = new IPerk[]
            {

                //Накладывает [[Обморожение]] на 20 врагов в радиусе 50 п. раз в 10 с.


            }
        };
    }
    public Item TacticExoskeleton()//Экзоскелет тактика
    {
        return new Item(LocaleKeys.Main.i_TacticExoskeleton)
        {
            Type = ItemType.TacticExoskeleton,
            Cost = 320,
            Icon = Addresses.Ico_TacticExoskeleton,
            Perks = new IPerk[]
            {

                new SimplePerk(PerkType.PlayerMaxHP,
                    new StatModificator(10f, StatModificatorType.Additive, StatType.MaxHP)),//[[Мак хит поинтов]] + 10 ед.
                //При получении урона по персонажу выпускает волну [[Обморожение]] радиусом 20 п.
                new SimplePerk(PerkType.PlayerFireResist,
                               new StatModificator(.5f, StatModificatorType.Additive, StatType.FireAbsorption))//[[Огонь]] не наносит урона.//[[Огонь]] наносит персонажу на 50% урона меньше.
                //[[Экзоскелет]].


            }
        };
    }
    public Item FullUnloading()//Полная разгрузка
    {
        return new Item(LocaleKeys.Main.i_FullUnloading)
        {
            Type = ItemType.FullUnloading,
            Cost = 320,
            Icon = Addresses.Ico_FullUnloading,
            Perks = new IPerk[]
            {

                //Раз в 4 с. в случайную точку [[Зона контроля]] падает снаряд, выпускает волну [[Ядовитый газ]] радиусом 15 п.


            }
        };
    }
    public Item MeteorRain()//Метеоритный дождь
    {
        return new Item(LocaleKeys.Main.i_MeteorRain)
        {
            Type = ItemType.MeteorRain,
            Cost = 320,
            Icon = Addresses.Ico_MeteorRain,
            Perks = new IPerk[]
            {

                //Раз в 4 с. в случайную точку [[Зона контроля]] падает снаряд, наносящий 60 ед. [[Физический урон]] в центре, и 20 ед. по бокам.
                //Радиус поражения - 10 п.


            }
        };
    }
    public Item Arsenal()//Арсенал
    {
        return new Item(LocaleKeys.Main.i_Arsenal)
        {
            Type = ItemType.Arsenal,
            Cost = 320,
            Icon = Addresses.Ico_Arsenal,
            Perks = new IPerk[]
            {

                _perksFactory.Create<PerkPBarrels>(4f),//Раз в 4 с. сбрасывает [[Ящик со взрывчаткой]].


            }
        };
    }
    public Item OrbitalSupportSystem()//Система орбитальной поддержки
    {
        return new Item(LocaleKeys.Main.i_OrbitalSupportSystem)
        {
            Type = ItemType.OrbitalSupportSystem,
            Cost = 320,
            Icon = Addresses.Ico_OrbitalSupportSystem,
            Perks = new IPerk[]
            {

                new SimplePerk(PerkType.RayError,
                              new StatModificator(-4, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] уменьшается на -4.
                //Уменьшает стоимсоть луча за пройденную длину луча.
                //Раз в 14 с. сбрасывает [[Ящик со взрывчаткой]].
                //После взрыва оставляет после себя хилку.
                //[[Востановление хп]] 1 ед.


            }
        };
    }
    public Item AtomicRay()//Атомный луч
    {
        return new Item(LocaleKeys.Main.i_AtomicRay)
        {
            Type = ItemType.AtomicRay,
            Cost = 320,
            Icon = Addresses.Ico_AtomicRay,
            Perks = new IPerk[]
            {

                //Луч прекрощяет бить фотонным уроном.
                //Он накладывает статус [[Радиация]] с уроном в 50% от [[Базовый урон луча]].


            }
        };
    }
    public Item OverloadedRay()//Перегруженный луч
    {
        return new Item(LocaleKeys.Main.i_OverloadedRay)
        {
            Type = ItemType.OverloadedRay,
            Cost = 320,
            Icon = Addresses.Ico_OverloadedRay,
            Perks = new IPerk[]
            {

                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(1f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +100%.
                new SimplePerk(PerkType.RayDelay,
                               new StatModificator(2, StatModificatorType.Additive, StatType.RayDelay))//[[Задержка активации луча]] +2 секунды.


            }
        };
    }
    public Item ChemonuclearReactor()//Хемоядерный реактор
    {
        return new Item(LocaleKeys.Main.i_ChemonuclearReactor)
        {
            Type = ItemType.ChemonuclearReactor,
            Cost = 320,
            Icon = Addresses.Ico_ChemonuclearReactor,
            Perks = new IPerk[]
            {

                //Генерация 10% от максимального заряда энергии в сек.


            }
        };
    }
    public Item Lightning()//Молния
    {
        return new Item(LocaleKeys.Main.i_Lightning)
        {
            Type = ItemType.Lightning,
            Cost = 320,
            Icon = Addresses.Ico_Lightning,
            Perks = new IPerk[]
            {

                //Луч прекрощяет бить фотонным уроном.


            }
        };
    }
    public Item HighCapacityAccumulator()//Аккумулятор большой ёмкости
    {
        return new Item(LocaleKeys.Main.i_HighCapacityAccumulator)
        {
            Type = ItemType.HighCapacityAccumulator,
            Cost = 320,
            Icon = Addresses.Ico_HighCapacityAccumulator,
            Perks = new IPerk[]
            {

                new SimplePerk(PerkType.RayPathLenght,
                               new StatModificator(2f, StatModificatorType.Multiplicative, StatType.Capacity))//[[Базовая ёмкость энергии]] + 200%.


            }
        };
    }
    public Item ExtraDriveUnits()//Экстраприводы
    {
        return new Item(LocaleKeys.Main.i_ExtraDriveUnits)
        {
            Type = ItemType.ExtraDriveUnits,
            Cost = 320,
            Icon = Addresses.Ico_ExtraDriveUnits,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(2, StatModificatorType.Additive, StatType.RaySpeed),//[[Базовая скорость луча]] +200%.
                               new StatModificator(3, StatModificatorType.Additive, StatType.RayError)) //Погрешность +3п.
               
            }
        };
    }
    public Item ReverseSystemTwo()//Система обратного хода
    {
        return new Item(LocaleKeys.Main.i_ReverseSystemTwo)
        {
            Type = ItemType.ReverseSystemTwo,
            Cost = 320,
            Icon = Addresses.Ico_ReverseSystemTwo,
            Perks = new IPerk[]
            {
                new SimplePerk(PerkType.RaySpeed,
                               new StatModificator(1.3f, StatModificatorType.Additive, StatType.RayError)),//[[Базовая скорость луча]] +130%.
                new SimplePerk(PerkType.RayReverse,
                               new StatModificator(1f, StatModificatorType.Additive, StatType.RayReverse))//Луч проходит свой путь дважды.
            }
        };
    }
    public Item PillarOfLight()//Столб света
    {
        return new Item(LocaleKeys.Main.i_PillarOfLight)
        {
            Type = ItemType.PillarOfLight,
            Cost = 320,
            Icon = Addresses.Ico_PillarOfLight,
            Perks = new IPerk[]
            {

                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(-.9f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] - 90%.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(2.5f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius))//[[Радиус поражения луча]] +250 %


            }
        };
    }
    public Item RaySplitter()//Расщепитель луча
    {
        return new Item(LocaleKeys.Main.i_RaySplitter)
        {
            Type = ItemType.RaySplitter,
            Cost = 320,
            Icon = Addresses.Ico_RaySplitter,
            Perks = new IPerk[]
            {

                //Создает 3 луча вместо одного, [[Базовый урон луча]] которых равен 40%, а [[Радиус поражения луча]]  60%.
                //Все дополнительные параметры применяются ко всем 3м лучам.


            }
        };
    }
    public Item ArchimedesMirror()//Зеркало Архимеда
    {
        return new Item(LocaleKeys.Main.i_ArchimedesMirror)
        {
            Type = ItemType.ArchimedesMirror,
            Cost = 320,
            Icon = Addresses.Ico_ArchimedesMirror,
            Perks = new IPerk[]
            {

                new SimplePerk(PerkType.RayDamage,
                               new StatModificator(1.2f, StatModificatorType.Multiplicative, StatType.RayDamage)),//[[Базовый урон луча]] +120%.
                new SimplePerk(PerkType.RayDamageAreaRadius,
                               new StatModificator(-0.8f, StatModificatorType.Multiplicative, StatType.RayDamageAreaRadius)),//[[Радиус поражения луча]] -80 %.
                new SimplePerk(PerkType.RayError,
                              new StatModificator(1, StatModificatorType.Additive, StatType.RayError))//[[Погрешность позиционирования луча]] +1п.


            }
        };
    }
    public Item FireRay()//Огненный луч
    {
        return new Item(LocaleKeys.Main.i_FireRay)
        {
            Type = ItemType.FireRay,
            Cost = 320,
            Icon = Addresses.Ico_FireRay,
            Perks = new IPerk[]
            {

                //Луч прекрощяет бить фотонным уроном.
                //Он бьет [[Огонь]] с уроном в 50% от [[Базовый урон луча]].


            }
        };
    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////белые арты
    /// </summary>
    /// <returns></returns>
    public Item Empty()//Пустышка
    {
        return new Item(LocaleKeys.Main.i_Empty)
        {
            Type = ItemType.Empty,
            Cost = 40,
            Icon = Addresses.Ico_Empty,
            Perks = new IPerk[]
            {
                //Ничего не делает.
            }
        };
    }
    public Item FireGem()//Огненный самоцвет
    {
        return new Item(LocaleKeys.Main.i_FireGem)
        {
            Type = ItemType.FireGem,
            Cost = 40,
            Icon = Addresses.Ico_FireGem,
            Perks = new IPerk[]
            {
                //Раз в 10 секунды создает 5 искр, наносяжих 20 ед. урона [[Огонь]].
                //Искры летят от персонажа на растояние в 40 п.
            }
        };
    }
    public Item RadiantMineral()//Лучистый минерал
    {
        return new Item(LocaleKeys.Main.i_RadiantMineral)
        {
            Type = ItemType.RadiantMineral,
            Cost = 40,
            Icon = Addresses.Ico_RadiantMineral,
            Perks = new IPerk[]
            {
                //Накладывает статус [[Радиация]] в радиусе 10 п. от персонажа с уроном в 4 единицы  раз в 2 с. в течение 8 с.
            }
        };
    }
    public Item DistortingCrystal()//Искажающий хрусталь
    {
        return new Item(LocaleKeys.Main.i_DistortingCrystal)
        {
            Type = ItemType.DistortingCrystal,
            Cost = 40,
            Icon = Addresses.Ico_DistortingCrystal,
            Perks = new IPerk[]
            {
                //Наносимый персонажу урон увеличивается на 1.
                //[[Минерал]].
            }
        };
    }
    public Item ElectricalLead()//Электрическая жила
    {
        return new Item(LocaleKeys.Main.i_ElectricalLead)
        {
            Type = ItemType.ElectricalLead,
            Cost = 40,
            Icon = Addresses.Ico_ElectricalLead,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkElectricalLead>(6f)//Раз в 6 с. выпускает в случайного врага молнию, бьющую 20 ед. [[Электричество]].
                //[[Минерал]].
            }
        };
    }
    public Item GravityStone()//Гравитационный камень
    {
        return new Item(LocaleKeys.Main.i_GravityStone)
        {
            Type = ItemType.GravityStone,
            Cost = 40,
            Icon = Addresses.Ico_GravityStone,
            Perks = new IPerk[]
            {
                new PerkGravityMatter(15, 2, 10, .5f, 1, PerkType.GravityMatter)
                //Вращает осколок материи вокруг персонажа в радиусе 15 п. Осколок наносит 10 ед.урона [[Физический урон]] и оглушает на 0.5 секунды.
                //[[Ксеноминерал]].
            }
        };
    }
    public Item PurpleTuber()//Фиолетовый клубешь
    {
        return new Item(LocaleKeys.Main.i_PurpleTuber)
        {
            Type = ItemType.PurpleTuber,
            Cost = 40,
            Icon = Addresses.Ico_PurpleTuber,
            Perks = new IPerk[]
            {
                //При получении урона по персонажу выпускает волну [[Ядовитый газ]] радиусом 20 п.
                //[[Минерал]].
            }
        };
    }
    public Item PoleOfCold()//Полюс холода
    {
        return new Item(LocaleKeys.Main.i_PoleOfCold)
        {
            Type = ItemType.PoleOfCold,
            Cost = 40,
            Icon = Addresses.Ico_PoleOfCold,
            Perks = new IPerk[]
            {
                _perksFactory.Create<PerkColdAOEWithCooldown>(5, 40f, 10f)
                //new PerkColdAOE(5, 40, 10)
                //Накладывает [[Обморожение]] на 5 врагов в радиусе 40 п. раз в 10 с.
                //[[Минерал]].
            }
        };
    }
    public Item StoneApple()//Каменное яблоко
    {
        return new Item(LocaleKeys.Main.i_StoneApple)
        {
            Type = ItemType.StoneApple,
            Cost = 40,
            Icon = Addresses.Ico_StoneApple,
            Perks = new IPerk[]
            {
                //[[Востановление хп]]  +1 раз в 30 с.
                //[[Минерал]].
            }
        };
    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
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
                _perksFactory.Create<PerkPEnergyAbsorber>(5f)//урон, длительность
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
