using System;
using UnityEngine.Localization;

public class ItemTemplate
{
    public ItemId Id;
    public int UpgradeCount;
    public bool Unique;
    public int Cost;
    public string IconAddress = Addresses.Ico_Battery;
    public LocalizedString NameLocalizedString;
    public LocalizedString DescriptionLocalizedString;
    public Func<IPerk[]> CreatePerksFunc;

    public ItemTemplate(LocalizedString name, LocalizedString description = null)
    {
        NameLocalizedString = name;
        DescriptionLocalizedString = description;
    }
    public IPerk[] CreatePerks()
    {
        return CreatePerksFunc?.Invoke()?? new IPerk[0];
    }
    public Item CreateItem()
    {
        return new Item(this);
    }
}
