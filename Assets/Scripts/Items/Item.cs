using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class Item : WithId
{
    public ItemType Type;
    public int UpgradeCount = 0;
    public bool Unique = false;
    public string Description => UpdateDescription();
    public string Name => UpdateName();
    public int Cost = 10;

    public IPerk[] Perks = { };
    public string Icon = Addresses.Ico_Battery;

    private LocalizedString _localizedName;
    private LocalizedString _localizedDescription;
    private string _description = null;

    public int SellCost => Cost / 2;
    private string _name = null;

    private string UpdateName()
    {
        _name = _localizedName.GetLocalizedString() + new string('+', UpgradeCount);
        return _name;
    }
    private string UpdateDescription()
    {
        _description = "";
        if (_localizedDescription != null)
            _description = _localizedDescription.GetLocalizedString() + "\n";
        foreach (var perk in Perks)
        {
            string descr = perk.GetDescription();
            if (string.IsNullOrEmpty(descr))
                continue;
            _description += descr + "\n";
        }
        return _description;
    }
    public Item(LocalizedString name, LocalizedString description = null)
    {
        _localizedName = name;
        _localizedDescription = description;
    }
}
