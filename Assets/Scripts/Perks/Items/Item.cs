using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : WithId
{
    public string Name => "�������";
    public string Description => "������ ���� +5 �";
    public int Cost { get; private set; } = 10;
    public IPerk Perk { get; private set; } = PlayerPerksDB.RayPathLenghtPerk();
    public AddressKeys AddressKeys { get; private set; } = AddressKeys.Ico_Battery;
}
