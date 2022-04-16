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
        _uiInventory.OnAdded += Inventory.Add;
        _uiInventory.OnRemoved += Inventory.Remove;
        _uiStash.OnAdded += Stash.Add;
        _uiStash.OnRemoved += Stash.Remove;
    }
}
