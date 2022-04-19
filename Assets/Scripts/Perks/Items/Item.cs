using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : WithId
{
    public string Name = "�������";
    public string Description = "������ ���� +5 �";
    public int Cost = 10;
    public IPerk[] Perks = { PlayerPerksDB.RayPathLenghtPerk() };
    public AddressKeys Icon = AddressKeys.Ico_Battery;
}
