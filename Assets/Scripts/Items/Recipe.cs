public struct Recipe
{
    public ItemId Component1;
    public ItemId Component2;
    public ItemId Result;

    public Recipe(ItemId component1, ItemId component2, ItemId result)
    {
        Component1 = component1;
        Component2 = component2;
        Result = result;
    }

    public ItemId GetSecondComponent(ItemId type)
    {
        if (type == Component1)
            return Component2;
        if (type == Component2)
            return Component1;
        return ItemId.None;
    }
}
