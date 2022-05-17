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
