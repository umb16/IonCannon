using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIPlayerInventory : BaseLayer
{
    [SerializeField] UIInventory _uiInventory;
    [SerializeField] UIInventory _uiStash;
    private Inventory Inventory => _player.Value.Inventory;
    private Inventory Stash => _player.Value.Stash;
    private AsyncReactiveProperty<Player> _player;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;
        _player.Where(x => x != null).ForEachAsync(x =>
        {
            SetInventory(x.Inventory, _uiInventory);
            _uiInventory.IsActiveInventory = true;
            SetInventory(x.Stash, _uiStash);
        });
    }

    protected override void OnFinishHiding()
    {
        base.OnFinishHiding();
        TooltipController.Instance.UnassignTooltip();
    }

    private void SetInventory(Inventory inventory, UIInventory uiInventory)
    {
        inventory.ItemAdded += uiInventory.AddItem;
        inventory.ItemRemoved += uiInventory.RemoveItem;
        uiInventory.SetRealInventory(inventory);
        uiInventory.PlayerInventory = this;
    }
    public void HighlightItems(ItemType itemType, UIInventorySlot exception = null)
    {
        _uiInventory.HighlightItems(itemType, exception);
        _uiStash.HighlightItems(itemType, exception);
    }
}
