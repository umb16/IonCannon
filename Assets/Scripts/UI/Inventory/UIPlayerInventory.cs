using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using Zenject;

public class UIPlayerInventory : MonoBehaviour
{
    [SerializeField] UIInventory _uiInventory;
    [SerializeField] UIInventory _uiStash;
    private Inventory Inventory => _player.Value.Inventory;
    private Inventory Stash => _player.Value.Stash;
    private AsyncReactiveProperty<Player> _player;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, GameData gameData)
    {
        _player = player;
        _player.Where(x => x != null).ForEachAsync(x =>
        {
            SetInventory(x.Inventory, _uiInventory);
            _uiInventory.IsActiveInventory = true;
            SetInventory(x.Stash, _uiStash);
        });

    }
    private void OnDisable()
    {
        TooltipController.Instance.UnassignTooltip();
    }

    private void SetInventory(Inventory inventory, UIInventory uiInventory)
    {
        inventory.ItemAdded += uiInventory.AddItem;
        inventory.ItemRemoved += uiInventory.RemoveItem;
        inventory.SlotAdded += uiInventory.AddSlot;
        uiInventory.SetRealInventory(inventory);
        uiInventory.PlayerInventory = this;
    }
    public void HighlightItems(ItemId itemType, UIInventorySlot exception = null)
    {
        _uiInventory.HighlightItems(itemType, exception);
        _uiStash.HighlightItems(itemType, exception);
    }
}
