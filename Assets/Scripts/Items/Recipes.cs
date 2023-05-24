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
        
        new Recipe(ItemType.ArmoredPlates, ItemType.ArmoredPlates, ItemType.AdditionalArmorContour),
        new Recipe(ItemType.ArmoredPlates, ItemType.ExoskeletonModule, ItemType.StiffeningRibs),
        new Recipe(ItemType.ExoskeletonModule, ItemType.Accelerator, ItemType.MovableMechanisms),
        new Recipe(ItemType.Accelerator, ItemType.MagneticCore, ItemType.ReinforcedMagneticCore),
        new Recipe(ItemType.Accelerator, ItemType.OxygenCylinders, ItemType.OxygenSprayer),
        new Recipe(ItemType.ExoskeletonModule, ItemType.CoprocessorTwo, ItemType.AnalyzingModule),
        new Recipe(ItemType.FreonCylinders, ItemType.Accelerator, ItemType.FreonSprayer),
        new Recipe(ItemType.ExoskeletonModule, ItemType.OxygenCylinders, ItemType.OxygenSupplyModule),
        new Recipe(ItemType.OxygenCylinders, ItemType.FreonCylinders, ItemType.PoisonGasCylinders),
        new Recipe(ItemType.CoprocessorTwo, ItemType.DeliveryDeviceTwo, ItemType.ProcessingSlagDropping),
        new Recipe(ItemType.AdditionalDriveUnits, ItemType.Accumulator, ItemType.HighSpeedDriveUnits),
        new Recipe(ItemType.AdditionalDriveUnits, ItemType.OxygenCylinders, ItemType.DeliveryOfExplosives),
        new Recipe(ItemType.AdditionalDriveUnits, ItemType.AtomicCell, ItemType.IonizingPlant),
        new Recipe(ItemType.AtomicCell, ItemType.FreonCylinders, ItemType.StableAtomicCell),
        new Recipe(ItemType.Accumulator, ItemType.Accumulator, ItemType.AccumulatorBattery),
        new Recipe(ItemType.Coprocessor, ItemType.Accumulator, ItemType.PowerControllerTwo),
        new Recipe(ItemType.AdditionalLens, ItemType.AdditionalDriveUnits, ItemType.DiffusionObjective),
        new Recipe(ItemType.Coprocessor, ItemType.AdditionalDriveUnits, ItemType.ErrorCalculator),
        new Recipe(ItemType.AdditionalLens, ItemType.AdditionalLens, ItemType.FocusingObjective),
        new Recipe(ItemType.OxygenCylinders, ItemType.AdditionalDriveUnits, ItemType.PlasmaSupercharger),
                   
        new Recipe(ItemType.DistortingCrystal, ItemType.AnalyzingModule, ItemType.DistortionMirror),
        new Recipe(ItemType.AdditionalArmorContour, ItemType.PurpleTuber, ItemType.PurpleCovering),
        new Recipe(ItemType.AdditionalArmorContour, ItemType.AnalyzingModule, ItemType.ExoskeletonProtection),
        new Recipe(ItemType.AdditionalArmorContour, ItemType.StiffeningRibs, ItemType.ReinforcedCarcass),
        new Recipe(ItemType.StiffeningRibs, ItemType.MovableMechanisms, ItemType.ExoskeletonBooster),
        new Recipe(ItemType.MovableMechanisms, ItemType.ReinforcedMagneticCore, ItemType.MagneticManipulatorTwo),
        new Recipe(ItemType.ElectricalLead, ItemType.ReinforcedMagneticCore, ItemType.ElectromagneticFieldCore),
        new Recipe(ItemType.ReinforcedMagneticCore, ItemType.GravityStone, ItemType.StarDust),
        new Recipe(ItemType.GravityStone, ItemType.AnalyzingModule, ItemType.StarShard),
        new Recipe(ItemType.OxygenSprayer, ItemType.AnalyzingModule, ItemType.OxidationSystem),
        new Recipe(ItemType.AnalyzingModule, ItemType.FreonSprayer, ItemType.FireSystem),
        new Recipe(ItemType.FreonSprayer, ItemType.FireGem, ItemType.DielectricGasSource),
        new Recipe(ItemType.PoisonGasCylinders, ItemType.PurpleTuber, ItemType.ConcentratedPoisonGasCylinders),
        new Recipe(ItemType.AnalyzingModule, ItemType.PurpleTuber, ItemType.OrganicDecompositionDevice),
        new Recipe(ItemType.MovableMechanisms, ItemType.OxygenSupplyModule, ItemType.OxygenSaturationModule),
        new Recipe(ItemType.AnalyzingModule, ItemType.StoneApple, ItemType.TreatmentSystem),
        new Recipe(ItemType.AnalyzingModule, ItemType.Empty, ItemType.Echo),
        new Recipe(ItemType.AnalyzingModule, ItemType.FireGem, ItemType.HandmadeFlamethrower),
        new Recipe(ItemType.FreonSprayer, ItemType.PoleOfCold, ItemType.WhiteShroud),
        new Recipe(ItemType.AnalyzingModule, ItemType.PoleOfCold, ItemType.IceSoul),
        new Recipe(ItemType.PurpleTuber, ItemType.ProcessingSlagDropping, ItemType.PoisonousSubstancesDropping),
        new Recipe(ItemType.ProcessingSlagDropping, ItemType.HighSpeedDriveUnits, ItemType.NonConformingMaterialsDropping),
        new Recipe(ItemType.HighSpeedDriveUnits, ItemType.DeliveryOfExplosives, ItemType.ExplosiveMixtureDropping),
        new Recipe(ItemType.DeliveryOfExplosives, ItemType.StoneApple, ItemType.BakedApplesDelivery),
        new Recipe(ItemType.RadiantMineral, ItemType.IonizingPlant, ItemType.RadiationContour),
        new Recipe(ItemType.IonizingPlant, ItemType.StableAtomicCell, ItemType.IsotopeReactor),
        new Recipe(ItemType.StableAtomicCell, ItemType.PoleOfCold, ItemType.SmallNuclearReactor),
        new Recipe(ItemType.IonizingPlant, ItemType.ElectricalLead, ItemType.AtmosphericIonizer),
        new Recipe(ItemType.ElectricalLead, ItemType.AccumulatorBattery, ItemType.SelfChargingAccumulator),
        new Recipe(ItemType.StableAtomicCell, ItemType.PowerControllerTwo, ItemType.RayOverloadController),
        new Recipe(ItemType.HighSpeedDriveUnits, ItemType.AccumulatorBattery, ItemType.ElectricDriveUnits),
        new Recipe(ItemType.AccumulatorBattery, ItemType.PowerControllerTwo, ItemType.MediumCapacityAccumulator),
        new Recipe(ItemType.PowerControllerTwo, ItemType.ErrorCalculator, ItemType.ErrorCorrector),
        new Recipe(ItemType.DiffusionObjective, ItemType.DistortingCrystal, ItemType.CrystalHalo),
        new Recipe(ItemType.DiffusionObjective, ItemType.ErrorCalculator, ItemType.RayDiffusionSystem),
        new Recipe(ItemType.ErrorCalculator, ItemType.FocusingObjective, ItemType.RayFocusingSystem),
        new Recipe(ItemType.FocusingObjective, ItemType.DistortingCrystal, ItemType.CrystalLens),
        new Recipe(ItemType.PlasmaSupercharger, ItemType.FireGem, ItemType.FireFootprints),

        new Recipe(ItemType.PurpleCovering, ItemType.OrganicDecompositionDevice, ItemType.VeilOfDecay),
        new Recipe(ItemType.ExoskeletonProtection, ItemType.ReinforcedCarcass, ItemType.WarriorExoskeleton),
        new Recipe(ItemType.DistortionMirror, ItemType.ExoskeletonBooster, ItemType.SpaceShiftSystem),
        new Recipe(ItemType.ExoskeletonBooster, ItemType.OxygenSaturationModule, ItemType.PioneerExoskeleton),
        new Recipe(ItemType.MagneticManipulatorTwo, ItemType.StarDust, ItemType.GravitationalExplosionDevice),
        new Recipe(ItemType.ElectromagneticFieldCore, ItemType.StarDust, ItemType.PerpetualMotionMachine),
        new Recipe(ItemType.ElectromagneticFieldCore, ItemType.StarShard, ItemType.StarSatellite),
        new Recipe(ItemType.MagneticManipulatorTwo, ItemType.OxidationSystem, ItemType.HarvesterExoskeleton),
        new Recipe(ItemType.OxidationSystem, ItemType.FireSystem, ItemType.EnvironmentalControlSystem),
        new Recipe(ItemType.ElectromagneticFieldCore, ItemType.DielectricGasSource, ItemType.EngineerExoskeleton),
        new Recipe(ItemType.TreatmentSystem, ItemType.DielectricGasSource, ItemType.TissueRegenerationSystem),
        new Recipe(ItemType.ConcentratedPoisonGasCylinders, ItemType.HandmadeFlamethrower, ItemType.CombatFlamethrower),
        new Recipe(ItemType.OxygenSaturationModule, ItemType.TreatmentSystem, ItemType.LifeSupportingSystem),
        new Recipe(ItemType.Echo, ItemType.IceSoul, ItemType.GreatGlaciation),
        new Recipe(ItemType.ExoskeletonProtection, ItemType.WhiteShroud, ItemType.TacticExoskeleton),
        new Recipe(ItemType.PoisonousSubstancesDropping, ItemType.NonConformingMaterialsDropping, ItemType.FullUnloading),
        new Recipe(ItemType.NonConformingMaterialsDropping, ItemType.ElectricDriveUnits, ItemType.MeteorRain),
        new Recipe(ItemType.ExplosiveMixtureDropping, ItemType.ElectricDriveUnits, ItemType.Arsenal),
        new Recipe(ItemType.BakedApplesDelivery, ItemType.ErrorCorrector, ItemType.OrbitalSupportSystem),
        new Recipe(ItemType.RadiationContour, ItemType.IsotopeReactor, ItemType.AtomicRay),
        new Recipe(ItemType.IsotopeReactor, ItemType.RayOverloadController, ItemType.OverloadedRay),
        new Recipe(ItemType.SmallNuclearReactor, ItemType.AtmosphericIonizer, ItemType.ChemonuclearReactor),
        new Recipe(ItemType.AtmosphericIonizer, ItemType.SelfChargingAccumulator, ItemType.Lightning),
        new Recipe(ItemType.SelfChargingAccumulator, ItemType.MediumCapacityAccumulator, ItemType.HighCapacityAccumulator),
        new Recipe(ItemType.ElectricDriveUnits, ItemType.MediumCapacityAccumulator, ItemType.ExtraDriveUnits),
        new Recipe(ItemType.ElectricDriveUnits, ItemType.ErrorCorrector, ItemType.ReverseSystemTwo),
        new Recipe(ItemType.CrystalHalo, ItemType.RayDiffusionSystem, ItemType.PillarOfLight),
        new Recipe(ItemType.RayDiffusionSystem, ItemType.RayFocusingSystem, ItemType.RaySplitter),
        new Recipe(ItemType.RayFocusingSystem, ItemType.CrystalLens, ItemType.ArchimedesMirror),
        new Recipe(ItemType.RayOverloadController, ItemType.FireFootprints, ItemType.FireRay),
    };

    public static IEnumerable<Recipe> GetAllValidRecipes(ItemType type)
    {
        return _recipes.Where(x => x.GetSecondComponent(type) != ItemType.None);
    }
    public static IEnumerable<ItemType> GetAllSecondComponents(ItemType type)
    {
        return _recipes.Select(x => x.GetSecondComponent(type)).Where(x => x != ItemType.None);
    }

    public static ItemType GetResult(ItemType type1, ItemType type2)
    {
        var list = _recipes.Where(x => x.GetSecondComponent(type1) == type2);
        if (list.Any())
            return list.First().Result;
        return ItemType.None;
    }
}
