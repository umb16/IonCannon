using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : WithId
{
    public ItemType Type;
    public bool Unique = false;
    public string Name = "�������";
    public string Description = "������ ���� +10 �";
    public int Cost = 10;
    public int SellCost => Cost / 2;
    public IPerk[] Perks = { PlayerPerksDB.RayPathLenghtPerk() };
    public AddressKeys Icon = AddressKeys.Ico_Battery;
}