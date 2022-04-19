using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIPlayerInventory : MonoBehaviour
{
    [SerializeField] UIInventory _uiInventory;
    [SerializeField] UIInventory _uiStash;
    private Inventory Inventory => _player.Inventory;
    private Inventory Stash => _player.Stash;
    private Player _player;
    [Inject]
    private void Construct(Player player)
    {
        _player = player;
        SetInventory(Inventory, _uiInventory);
        SetInventory(Stash, _uiStash);
    }
    private void Start()
    {
        Inventory.Add(ItemsDB.Battery());
        Stash.Add(ItemsDB.AdditionalDrives());
        Stash.Add(new Item());
    }
    private void SetInventory(Inventory inventory, UIInventory uiInventory)
    {
        inventory.ItemAdded += uiInventory.AddItem;
        inventory.ItemRemoved += uiInventory.RemoveItem;
        uiInventory.RealInventory = inventory;
    }
}
