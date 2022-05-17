using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recipes
{
    public static Recipe[] _recipes =
    {
        new Recipe(ItemType.Battery, ItemType.Coprocessor, ItemType.PowerController),
        new Recipe(ItemType.FocusLens, ItemType.DivergingLens, ItemType.LensSystem),
        new Recipe(ItemType.AdditionalDrives, ItemType.AdditionalDrives, ItemType.SpeedDrives),
        new Recipe(ItemType.Coprocessor, ItemType.Coprocessor, ItemType.CoprocessorPlus),
        new Recipe(ItemType.CoprocessorPlus, ItemType.CoprocessorPlus, ItemType.CoprocessorPlusPlus),
        new Recipe(ItemType.Amplifier, ItemType.Amplifier, ItemType.AmplifierPlus),
        new Recipe(ItemType.AmplifierPlus, ItemType.AmplifierPlus, ItemType.AmplifierPlusPlus),
        new Recipe(ItemType.LensSystem, ItemType.LensSystem, ItemType.LensSystemPlus),
        new Recipe(ItemType.Battery, ItemType.Battery, ItemType.BatteryPlus),
        new Recipe(ItemType.BatteryPlus, ItemType.BatteryPlus, ItemType.BatteryPlusPlus),
        new Recipe(ItemType.DeliveryDevice, ItemType.DeliveryDevice, ItemType.DeliveryDevicePlus),
        new Recipe(ItemType.DeliveryDevicePlus, ItemType.DeliveryDevicePlus, ItemType.DeliveryDevicePlusPlus),
        new Recipe(ItemType.IonizationUnit, ItemType.IonizationUnit, ItemType.IonizationUnitPlus),
        new Recipe(ItemType.IonizationUnitPlus, ItemType.IonizationUnitPlus, ItemType.IonizationUnitPlusPlus),
        new Recipe(ItemType.PowerController, ItemType.PowerController, ItemType.PowerControllerPlus),
        new Recipe(ItemType.BatteryPlus, ItemType.CoprocessorPlus, ItemType.PowerControllerPlus),
        new Recipe(ItemType.PowerControllerPlus, ItemType.PowerControllerPlus, ItemType.PowerControllerPlusPlus),
        new Recipe(ItemType.BatteryPlusPlus, ItemType.CoprocessorPlusPlus, ItemType.PowerControllerPlusPlus),
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
