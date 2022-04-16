using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : WithId
{
    public string Name => Perk.Name;
    public string Description => Perk.Description;
    public int Cost { get; private set; } = 0;
    public IPerk Perk { get; private set; } = PlayerPerksDB.RayPathLenghtPerk();
    public AddressKeys AddressKeys { get; private set; } = AddressKeys.Ico_Battery;
}
