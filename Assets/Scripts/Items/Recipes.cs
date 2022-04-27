using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Recipe
{
    public ItemType Component1;
    public ItemType Component2;
    public ItemType Result;

    public Recipe(ItemType component1, ItemType component2, ItemType result)
    {
        Component1 = component1;
        Component2 = component2;
        Result = result;
    }

    public ItemType GetSecondComponent(ItemType type)
    {
        if (type == Component1)
            return Component2;
        if (type == Component2)
            return Component1;
        return ItemType.None;
    }
}

public class Recipes
{
    public static Recipe[] _recipes = 
        { 
        new Recipe(ItemType.Battery, ItemType.Coprocessor, ItemType.PowerController),
        new Recipe(ItemType.FocusLens, ItemType.DivergingLens, ItemType.LensSystem),
    };
    public static IEnumerable<Recipe> GetAllValidRecipes(ItemType type)
    {
        return _recipes.Where(x => x.GetSecondComponent(type) != ItemType.None);
    }
    public static IEnumerable<ItemType> GetAllSecondComponents(ItemType type)
    {
        return _recipes.Select(x => x.GetSecondComponent(type)).Where(x=> x != ItemType.None);
    }

    public static ItemType GetResult(ItemType type1, ItemType type2)
    {
        var list = _recipes.Where(x => x.GetSecondComponent(type1) == type2);
            if (list.Any())
            return list.First().Result;
        return ItemType.None;
    }
}
