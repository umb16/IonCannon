using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UIInventory : MonoBehaviour
{
    public bool IsActiveInventory;
    public Inventory RealInventory { private set; get; }
    public UIPlayerInventory PlayerInventory;
    [SerializeField] private GameObject _uiSlotPrefab;
    private List<UIInventorySlot> _slots = new List<UIInventorySlot>();
    private static UIInventorySlot _dragingItem;
    public static UIInventorySlot SelectedItem { get; private set; }
    private AsyncReactiveProperty<Player> _player;
    private ItemsDB _itemsDB;

    public bool IsFull => !RealInventory.FreeSlotAvailable;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, ItemsDB itemsDB)
    {
        _player = player;
        _itemsDB = itemsDB;
    }

    public void SetRealInventory(Inventory inventory)
    {
        RealInventory = inventory;
        foreach (var slot in _slots)
        {
            Destroy(slot.gameObject);
        }
        _slots.Clear();
        SetSlotsCount(inventory.SlotsCount);
    }

    public void SetSlotsCount(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddSlot();
        }
    }

    public void RemoveSlot(int index)
    {
        Destroy(_slots[index].gameObject);
        _slots.RemoveAt(index);
    }

    public void AddSlot()
    {
        int index = _slots.Count;
        var go = Instantiate(_uiSlotPrefab, transform);
        go.SetActive(true);
        var slot = go.GetComponent<UIInventorySlot>();
        slot.BeginDrag += x => OnBeginDrag(x, slot);
        slot.Drag += x => OnDrag(x, slot);
        slot.EndDrag += x => OnEndDrag(x, slot);
        slot.PointerEnter += x => OnPointerEnter(x, slot);
        slot.PointerExit += OnPointerExit;
        slot.RemoveFromInventoryAction = RealInventory.Remove;
        slot.AddToInventoryAction = RealInventory.Add;
        if (RealInventory.TryGetItem(index, out Item item))
            slot.Set(item).Forget();
        _slots.Add(slot);
    }

    private void OnPointerExit(PointerEventData eventData)
    {
        TooltipController.Instance.UnassignTooltip();
        if (_dragingItem == null)
            PlayerInventory.HighlightItems(ItemType.None);
    }

    private void OnPointerEnter(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty)
            return;
        if (_dragingItem != null)
        {
            if (_dragingItem != slot)
            {
                var result = Recipes.GetResult(slot.Item.Type, _dragingItem.Item.Type);
                if (result != ItemType.None)
                {
                    ShowTooltip(_itemsDB.CreateByType(result), true);
                }
            }
        }
        else
        {
            SelectedItem = slot;
            ShowTooltip(slot.Item);
            PlayerInventory.HighlightItems(slot.Item.Type, slot);
        }
    }

    private static void ShowTooltip(Item item, bool newItem = false)
    {
        string tooltiptext = string.Empty;
        if (newItem)
            tooltiptext += "<color=green><size=30>–≈«”À‹“¿“</size></color>\n";
        if (item.Unique)
            TooltipController.Instance.
            AssignTooltip(tooltiptext + @$"<color=yellow><size=30>{item.Name}</size></color>
<color=red>{LocaleKeys.Main.UNIQUE.GetLocalizedString()}</color>
{item.Description}");
        else
            TooltipController.Instance.
                AssignTooltip(tooltiptext + @$"<color=yellow><size=30>{item.Name}</size></color>
{item.Description}");
    }

    private void OnEndDrag(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty)
            return;
        var result = eventData.pointerCurrentRaycast;
        if (!TryRecycle(slot, result))
            if (!TryCombine(slot, result))
                TryPut(slot, result);
        slot.ImageTransform.SetParent(slot.transform);
        //slot.ImageTransform.SetSiblingIndex(slot.ImageTransform.GetSiblingIndex()-1);
        slot.ImageTransform.localPosition = Vector3.zero;
        PlayerInventory.HighlightItems(ItemType.None);
        _dragingItem = null;
    }

    private bool TryPut(UIInventorySlot slot, RaycastResult result)
    {
        var inventory = result.gameObject?.GetComponentInParent<UIInventory>();
        if (inventory != null && this != inventory && !inventory.IsFull &&
            (!inventory.IsActiveInventory || !slot.Item.Unique || !inventory.RealInventory.ContainsByType(slot.Item.Type)))
        {
            inventory.RealInventory.Add(slot.Item);
            RealInventory.Remove(slot.Item);
            SoundManager.Instance.PlayChangeSlot();
            return true;
        }
        return false;
    }

    private bool TryRecycle(UIInventorySlot slot, RaycastResult result)
    {
        var trash = result.gameObject?.GetComponentInParent<UIInventoryTrashCan>();
        if (trash != null)
        {
            _player.Value.Gold.AddBaseValue(slot.Item.SellCost);
            RealInventory.Remove(slot.Item);
            trash.PlaySound();
            return true;
        }
        return false;
    }

    private bool TryCombine(UIInventorySlot slot, RaycastResult result)
    {
        var hoveredSlot = result.gameObject?.GetComponentInParent<UIInventorySlot>();
        if (hoveredSlot != null && slot != hoveredSlot && !hoveredSlot.IsEmpty)
        {
            var resultType = Recipes.GetResult(slot.Item.Type, hoveredSlot.Item.Type);
            if (resultType != ItemType.None)
            {
                var newItem = _itemsDB.CreateByType(resultType);
                hoveredSlot.RemoveFromInventory();
                slot.RemoveFromInventory();
                _player.Value.AddItemDirectly(newItem);
                SoundManager.Instance.PlayTransmutate();
                //hoveredSlot.AddToInventory(newItem);
                return true;
            }
        }
        return false;
    }

    private void OnBeginDrag(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty)
            return;
        _dragingItem = slot;
        TooltipController.Instance.UnassignTooltip();
        slot.ImageTransform.SetParent(transform.parent.parent, true);
    }
    private void OnDrag(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty)
            return;
        var result = eventData.pointerCurrentRaycast;
        slot.ImageTransform.position = eventData.position;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].IsEmpty)
            {
                _slots[i].Set(item).Forget();
                break;
            }
        }
    }
    public void RemoveItem(Item item)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            var slot = _slots[i];
            if (slot.Item == item)
            {
                slot.Clear();
                break;
            }
        }
    }

    public void HighlightItems(ItemType type, UIInventorySlot exception = null)
    {
        var validComponents = Recipes.GetAllSecondComponents(type);

        foreach (var slot in _slots)
        {
            if (exception == slot || type == ItemType.None)
            {
                slot.Normal();
                continue;
            }
            if (validComponents.Count() == 0)
            {
                slot.Normal();
                continue;
            }
            foreach (var component in validComponents)
            {

                if (!slot.IsEmpty && slot.Item.Type == component)
                {
                    slot.Highlighted();
                }
                else
                {
                    slot.Normal();
                }
            }
        }
    }
}
