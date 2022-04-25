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
        _uiInventory.IsActiveInventory = true;
        SetInventory(Stash, _uiStash);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Start()
    {
        Inventory.Add(ItemsDB.Battery());
        /*Stash.Add(ItemsDB.Coprocessor());
        Stash.Add(ItemsDB.ExoskeletonSpeedBooster());
        Stash.Add(ItemsDB.Amplifier());
        Stash.Add(ItemsDB.FocusLens());
        Stash.Add(ItemsDB.FocusLens());
        Stash.Add(ItemsDB.DeliveryDevice());*/
    }
    private void SetInventory(Inventory inventory, UIInventory uiInventory)
    {
        inventory.ItemAdded += uiInventory.AddItem;
        inventory.ItemRemoved += uiInventory.RemoveItem;
        uiInventory.RealInventory = inventory;
        uiInventory.PlayerInventory = this;
    }
    public void HighlightItems(ItemType itemType, UIInventorySlot exception = null)
    {
        _uiInventory.HighlightItems(itemType, exception);
        _uiStash.HighlightItems(itemType, exception);
    }
}
