using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recipes
{
    public static Recipe[] _recipes =
    {
        //new Recipe(ItemType.Battery, ItemType.Coprocessor, ItemType.PowerController),
        //new Recipe(ItemType.FocusLens, ItemType.DivergingLens, ItemType.LensSystem),
        //new Recipe(ItemType.AdditionalDrives, ItemType.AdditionalDrives, ItemType.SpeedDrives),
        //new Recipe(ItemType.Coprocessor, ItemType.Coprocessor, ItemType.CoprocessorPlus),
        //new Recipe(ItemType.CoprocessorPlus, ItemType.CoprocessorPlus, ItemType.CoprocessorPlusPlus),
        //new Recipe(ItemType.Amplifier, ItemType.Amplifier, ItemType.AmplifierPlus),
        //new Recipe(ItemType.AmplifierPlus, ItemType.AmplifierPlus, ItemType.AmplifierPlusPlus),
        //new Recipe(ItemType.LensSystem, ItemType.LensSystem, ItemType.LensSystemPlus),
        //new Recipe(ItemType.Battery, ItemType.Battery, ItemType.BatteryPlus),
        //new Recipe(ItemType.BatteryPlus, ItemType.BatteryPlus, ItemType.BatteryPlusPlus),
        //new Recipe(ItemType.DeliveryDevice, ItemType.DeliveryDevice, ItemType.DeliveryDevicePlus),
        //new Recipe(ItemType.DeliveryDevicePlus, ItemType.DeliveryDevicePlus, ItemType.DeliveryDevicePlusPlus),
        //new Recipe(ItemType.IonizationUnit, ItemType.IonizationUnit, ItemType.IonizationUnitPlus),
        //new Recipe(ItemType.IonizationUnitPlus, ItemType.IonizationUnitPlus, ItemType.IonizationUnitPlusPlus),
        //new Recipe(ItemType.PowerController, ItemType.PowerController, ItemType.PowerControllerPlus),
        //new Recipe(ItemType.BatteryPlus, ItemType.CoprocessorPlus, ItemType.PowerControllerPlus),
        //new Recipe(ItemType.PowerControllerPlus, ItemType.PowerControllerPlus, ItemType.PowerControllerPlusPlus),
        //new Recipe(ItemType.BatteryPlusPlus, ItemType.CoprocessorPlusPlus, ItemType.PowerControllerPlusPlus),
        //new Recipe(ItemType.CoprocessorPlus, ItemType.SpeedDrives, ItemType.ReverseSystem),
        //new Recipe(ItemType.AtomicBattery, ItemType.AtomicBattery, ItemType.AtomicBatteryPlus),
        //new Recipe(ItemType.AtomicBatteryPlus, ItemType.AtomicBatteryPlus, ItemType.AtomicBatteryPlusPlus),
        
        new Recipe(ItemId.ArmoredPlates, ItemId.ArmoredPlates, ItemId.AdditionalArmorContour),
        new Recipe(ItemId.ArmoredPlates, ItemId.ExoskeletonModule, ItemId.StiffeningRibs),
        new Recipe(ItemId.ExoskeletonModule, ItemId.Accelerator, ItemId.MovableMechanisms),
        new Recipe(ItemId.Accelerator, ItemId.MagneticCore, ItemId.ReinforcedMagneticCore),
        new Recipe(ItemId.Accelerator, ItemId.OxygenCylinders, ItemId.OxygenSprayer),
        new Recipe(ItemId.ExoskeletonModule, ItemId.CoprocessorTwo, ItemId.AnalyzingModule),
        new Recipe(ItemId.FreonCylinders, ItemId.Accelerator, ItemId.FreonSprayer),
        new Recipe(ItemId.ExoskeletonModule, ItemId.OxygenCylinders, ItemId.OxygenSupplyModule),
        new Recipe(ItemId.OxygenCylinders, ItemId.FreonCylinders, ItemId.PoisonGasCylinders),
        new Recipe(ItemId.CoprocessorTwo, ItemId.DeliveryDeviceTwo, ItemId.ProcessingSlagDropping),
        new Recipe(ItemId.AdditionalDriveUnits, ItemId.Accumulator, ItemId.HighSpeedDriveUnits),
        new Recipe(ItemId.AdditionalDriveUnits, ItemId.OxygenCylinders, ItemId.DeliveryOfExplosives),
        new Recipe(ItemId.AdditionalDriveUnits, ItemId.AtomicCell, ItemId.IonizingPlant),
        new Recipe(ItemId.AtomicCell, ItemId.FreonCylinders, ItemId.StableAtomicCell),
        new Recipe(ItemId.Accumulator, ItemId.Accumulator, ItemId.AccumulatorBattery),
        new Recipe(ItemId.Coprocessor, ItemId.Accumulator, ItemId.PowerControllerTwo),
        new Recipe(ItemId.AdditionalLens, ItemId.AdditionalDriveUnits, ItemId.DiffusionObjective),
        new Recipe(ItemId.Coprocessor, ItemId.AdditionalDriveUnits, ItemId.ErrorCalculator),
        new Recipe(ItemId.AdditionalLens, ItemId.AdditionalLens, ItemId.FocusingObjective),
        new Recipe(ItemId.OxygenCylinders, ItemId.AdditionalDriveUnits, ItemId.PlasmaSupercharger),
                   
        new Recipe(ItemId.DistortingCrystal, ItemId.AnalyzingModule, ItemId.DistortionMirror),
        new Recipe(ItemId.AdditionalArmorContour, ItemId.PurpleTuber, ItemId.PurpleCovering),
        new Recipe(ItemId.AdditionalArmorContour, ItemId.AnalyzingModule, ItemId.ExoskeletonProtection),
        new Recipe(ItemId.AdditionalArmorContour, ItemId.StiffeningRibs, ItemId.ReinforcedCarcass),
        new Recipe(ItemId.StiffeningRibs, ItemId.MovableMechanisms, ItemId.ExoskeletonBooster),
        new Recipe(ItemId.MovableMechanisms, ItemId.ReinforcedMagneticCore, ItemId.MagneticManipulatorTwo),
        new Recipe(ItemId.ElectricalLead, ItemId.ReinforcedMagneticCore, ItemId.ElectromagneticFieldCore),
        new Recipe(ItemId.ReinforcedMagneticCore, ItemId.GravityStone, ItemId.StarDust),
        new Recipe(ItemId.GravityStone, ItemId.AnalyzingModule, ItemId.StarShard),
        new Recipe(ItemId.OxygenSprayer, ItemId.AnalyzingModule, ItemId.OxidationSystem),
        new Recipe(ItemId.AnalyzingModule, ItemId.FreonSprayer, ItemId.FireSystem),
        new Recipe(ItemId.FreonSprayer, ItemId.FireGem, ItemId.DielectricGasSource),
        new Recipe(ItemId.PoisonGasCylinders, ItemId.PurpleTuber, ItemId.ConcentratedPoisonGasCylinders),
        new Recipe(ItemId.AnalyzingModule, ItemId.PurpleTuber, ItemId.OrganicDecompositionDevice),
        new Recipe(ItemId.MovableMechanisms, ItemId.OxygenSupplyModule, ItemId.OxygenSaturationModule),
        new Recipe(ItemId.AnalyzingModule, ItemId.StoneApple, ItemId.TreatmentSystem),
        new Recipe(ItemId.AnalyzingModule, ItemId.Empty, ItemId.Echo),
        new Recipe(ItemId.AnalyzingModule, ItemId.FireGem, ItemId.HandmadeFlamethrower),
        new Recipe(ItemId.FreonSprayer, ItemId.PoleOfCold, ItemId.WhiteShroud),
        new Recipe(ItemId.AnalyzingModule, ItemId.PoleOfCold, ItemId.IceSoul),
        new Recipe(ItemId.PurpleTuber, ItemId.ProcessingSlagDropping, ItemId.PoisonousSubstancesDropping),
        new Recipe(ItemId.ProcessingSlagDropping, ItemId.HighSpeedDriveUnits, ItemId.NonConformingMaterialsDropping),
        new Recipe(ItemId.HighSpeedDriveUnits, ItemId.DeliveryOfExplosives, ItemId.ExplosiveMixtureDropping),
        new Recipe(ItemId.DeliveryOfExplosives, ItemId.StoneApple, ItemId.BakedApplesDelivery),
        new Recipe(ItemId.RadiantMineral, ItemId.IonizingPlant, ItemId.RadiationContour),
        new Recipe(ItemId.IonizingPlant, ItemId.StableAtomicCell, ItemId.IsotopeReactor),
        new Recipe(ItemId.StableAtomicCell, ItemId.PoleOfCold, ItemId.SmallNuclearReactor),
        new Recipe(ItemId.IonizingPlant, ItemId.ElectricalLead, ItemId.AtmosphericIonizer),
        new Recipe(ItemId.ElectricalLead, ItemId.AccumulatorBattery, ItemId.SelfChargingAccumulator),
        new Recipe(ItemId.StableAtomicCell, ItemId.PowerControllerTwo, ItemId.RayOverloadController),
        new Recipe(ItemId.HighSpeedDriveUnits, ItemId.AccumulatorBattery, ItemId.ElectricDriveUnits),
        new Recipe(ItemId.AccumulatorBattery, ItemId.PowerControllerTwo, ItemId.MediumCapacityAccumulator),
        new Recipe(ItemId.PowerControllerTwo, ItemId.ErrorCalculator, ItemId.ErrorCorrector),
        new Recipe(ItemId.DiffusionObjective, ItemId.DistortingCrystal, ItemId.CrystalHalo),
        new Recipe(ItemId.DiffusionObjective, ItemId.ErrorCalculator, ItemId.RayDiffusionSystem),
        new Recipe(ItemId.ErrorCalculator, ItemId.FocusingObjective, ItemId.RayFocusingSystem),
        new Recipe(ItemId.FocusingObjective, ItemId.DistortingCrystal, ItemId.CrystalLens),
        new Recipe(ItemId.PlasmaSupercharger, ItemId.FireGem, ItemId.FireFootprints),

        new Recipe(ItemId.PurpleCovering, ItemId.OrganicDecompositionDevice, ItemId.VeilOfDecay),
        new Recipe(ItemId.ExoskeletonProtection, ItemId.ReinforcedCarcass, ItemId.WarriorExoskeleton),
        new Recipe(ItemId.DistortionMirror, ItemId.ExoskeletonBooster, ItemId.SpaceShiftSystem),
        new Recipe(ItemId.ExoskeletonBooster, ItemId.OxygenSaturationModule, ItemId.PioneerExoskeleton),
        new Recipe(ItemId.MagneticManipulatorTwo, ItemId.StarDust, ItemId.GravitationalExplosionDevice),
        new Recipe(ItemId.ElectromagneticFieldCore, ItemId.StarDust, ItemId.PerpetualMotionMachine),
        new Recipe(ItemId.ElectromagneticFieldCore, ItemId.StarShard, ItemId.StarSatellite),
        new Recipe(ItemId.MagneticManipulatorTwo, ItemId.OxidationSystem, ItemId.HarvesterExoskeleton),
        new Recipe(ItemId.OxidationSystem, ItemId.FireSystem, ItemId.EnvironmentalControlSystem),
        new Recipe(ItemId.ElectromagneticFieldCore, ItemId.DielectricGasSource, ItemId.EngineerExoskeleton),
        new Recipe(ItemId.TreatmentSystem, ItemId.DielectricGasSource, ItemId.TissueRegenerationSystem),
        new Recipe(ItemId.ConcentratedPoisonGasCylinders, ItemId.HandmadeFlamethrower, ItemId.CombatFlamethrower),
        new Recipe(ItemId.OxygenSaturationModule, ItemId.TreatmentSystem, ItemId.LifeSupportingSystem),
        new Recipe(ItemId.Echo, ItemId.IceSoul, ItemId.GreatGlaciation),
        new Recipe(ItemId.ExoskeletonProtection, ItemId.WhiteShroud, ItemId.TacticExoskeleton),
        new Recipe(ItemId.PoisonousSubstancesDropping, ItemId.NonConformingMaterialsDropping, ItemId.FullUnloading),
        new Recipe(ItemId.NonConformingMaterialsDropping, ItemId.ElectricDriveUnits, ItemId.MeteorRain),
        new Recipe(ItemId.ExplosiveMixtureDropping, ItemId.ElectricDriveUnits, ItemId.Arsenal),
        new Recipe(ItemId.BakedApplesDelivery, ItemId.ErrorCorrector, ItemId.OrbitalSupportSystem),
        new Recipe(ItemId.RadiationContour, ItemId.IsotopeReactor, ItemId.AtomicRay),
        new Recipe(ItemId.IsotopeReactor, ItemId.RayOverloadController, ItemId.OverloadedRay),
        new Recipe(ItemId.SmallNuclearReactor, ItemId.AtmosphericIonizer, ItemId.ChemonuclearReactor),
        new Recipe(ItemId.AtmosphericIonizer, ItemId.SelfChargingAccumulator, ItemId.Lightning),
        new Recipe(ItemId.SelfChargingAccumulator, ItemId.MediumCapacityAccumulator, ItemId.HighCapacityAccumulator),
        new Recipe(ItemId.ElectricDriveUnits, ItemId.MediumCapacityAccumulator, ItemId.ExtraDriveUnits),
        new Recipe(ItemId.ElectricDriveUnits, ItemId.ErrorCorrector, ItemId.ReverseSystemTwo),
        new Recipe(ItemId.CrystalHalo, ItemId.RayDiffusionSystem, ItemId.PillarOfLight),
        new Recipe(ItemId.RayDiffusionSystem, ItemId.RayFocusingSystem, ItemId.RaySplitter),
        new Recipe(ItemId.RayFocusingSystem, ItemId.CrystalLens, ItemId.ArchimedesMirror),
        new Recipe(ItemId.RayOverloadController, ItemId.FireFootprints, ItemId.FireRay),
    };

    public static IEnumerable<Recipe> GetAllValidRecipes(ItemId type)
    {
        return _recipes.Where(x => x.GetSecondComponent(type) != ItemId.None);
    }
    public static IEnumerable<ItemId> GetAllSecondComponents(ItemId type)
    {
        return _recipes.Select(x => x.GetSecondComponent(type)).Where(x => x != ItemId.None);
    }

    public static ItemId GetResult(ItemId type1, ItemId type2)
    {
        var list = _recipes.Where(x => x.GetSecondComponent(type1) == type2);
        if (list.Any())
            return list.First().Result;
        return ItemId.None;
    }
}
