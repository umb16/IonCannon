using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIPlayerInventory : BaseLayer
{
    [SerializeField] UIInventory _uiInventory;
    [SerializeField] UIInventory _uiStash;
    private Inventory Inventory => _player.Inventory;
    private Inventory Stash => _player.Stash;
    private Player _player;
    private ItemsDB _itemsDB;

    [Inject]
    private void Construct(Player player, ItemsDB itemsDB)
    {
        _player = player;
        SetInventory(Inventory, _uiInventory);
        _uiInventory.IsActiveInventory = true;
        SetInventory(Stash, _uiStash);
        _itemsDB = itemsDB;
    }
    public void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        TooltipController.Instance.UnassignTooltip();
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
