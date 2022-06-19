using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : WithId
{
    public ItemType Type;
    public int UpgradeCount = 0;
    public bool NotForSale;
    public bool Unique = false;
    public string Name = "Батарея";
    public string Description = "Длинна пути +10 м";
    public int Cost = 10;
    public int SellCost => Cost / 2;
    public IPerk[] Perks = { PlayerPerksDB.RayPathLenghtPerk() };
    public string Icon = Addresses.Ico_Battery;
}
