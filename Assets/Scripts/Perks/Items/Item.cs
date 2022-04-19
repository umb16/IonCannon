using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : WithId
{
    public string Name = "Батарея";
    public string Description = "Длинна пути +5 м";
    public int Cost = 10;
    public IPerk[] Perks = { PlayerPerksDB.RayPathLenghtPerk() };
    public AddressKeys Icon = AddressKeys.Ico_Battery;
}
