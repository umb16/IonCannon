using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item /*: WithId*/
{
    public ItemId Id => _template.Id;
    public int UpgradeCount => _template.UpgradeCount;
    public bool Unique => _template.Unique;
    public string Description => GetDescription();
    public string Name => GetName();
    public int Cost => _template.Cost;

    public readonly IPerk[] Perks = { };
    public string IconAddress => _template.IconAddress;

    private ItemTemplate _template;
    public int SellCost => Cost / 2;

    private string GetName()
    {
        if (_template.NameLocalizedString == null)
            return "Error name";
        return _template.NameLocalizedString.GetLocalizedString() + new string('+', UpgradeCount);
    }
    private string GetDescription()
    {
        var description = "";
        if (_template.DescriptionLocalizedString != null)
            description = _template.DescriptionLocalizedString.GetLocalizedString() + "\n";
        foreach (var perk in Perks)
        {
            string descr = perk.GetDescription();
            if (string.IsNullOrEmpty(descr))
                continue;
            description += descr + "\n";
        }
        return description;
    }
    
    public Item(ItemTemplate itemTemplate)
    {
        _template = itemTemplate;
        Perks = _template.CreatePerks();
    }
}
