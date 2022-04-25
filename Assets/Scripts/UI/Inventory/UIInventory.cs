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
    public Inventory RealInventory;
    public UIPlayerInventory PlayerInventory;
    [SerializeField] private GameObject _uiSlotPrefab;
    private List<UIInventorySlot> _slots = new List<UIInventorySlot>();
    private static bool _draging;
    private static UIInventorySlot _dragingItem;
    private static UIInventorySlot _hoverItem;
    public static UIInventorySlot SelectedItem { get; private set; }
    private Player _player;
    public bool IsFull => !RealInventory.FreeSlotAvailable;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        SetSlotsCount(6);
    }

    public void SetSlotsCount(int count)
    {
        for (int i = 0; i < count; i++)
        {
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
            _slots.Add(slot);
        }
    }

    private void OnPointerExit(PointerEventData eventData)
    {
        TooltipController.Instance.UnassignTooltip();
    }

    private void OnPointerEnter(PointerEventData eventData, UIInventorySlot slot)
    {
        _hoverItem = null;
        if (slot.IsEmpty)
            return;
        if (_draging)
        {
            if (_dragingItem != slot)
            {
                var result = Recipes.GetResult(slot.Item.Type, _dragingItem.Item.Type);
                if (result != ItemType.None)
                {
                    ShowTooltip(ItemsDB.CreateByType(result));
                    _hoverItem = slot;
                }
            }
        }
        else
        {
            SelectedItem = slot;
            ShowTooltip(slot.Item);
            PlayerInventory.HighlightItems(slot.Item.Type);
        }
    }

    private static void ShowTooltip(Item item)
    {
        if (item.Unique)
            TooltipController.Instance.
            AssignTooltip(@$"<color=yellow><size=30>{item.Name}</size></color>
<color=red>”никально</color>
{item.Description}");
        else
            TooltipController.Instance.
                AssignTooltip(@$"<color=yellow><size=30>{item.Name}</size></color>
{item.Description}");
    }

    private void OnEndDrag(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty)
            return;
        if (_hoverItem != null && slot != _hoverItem)
        {
            var resultType = Recipes.GetResult(slot.Item.Type, _hoverItem.Item.Type);
            if (resultType != ItemType.None)
            {
                var newItem = ItemsDB.CreateByType(resultType);
                _hoverItem.RemoveFromInventory();
                slot.RemoveFromInventory();
                _hoverItem.AddToInventory(newItem);
            }
        }
        else
        {
            var result = eventData.pointerCurrentRaycast;
            var trash = result.gameObject.GetComponentInParent<UIInventoryTrashCan>();
            if (trash != null)
            {
                _player.Gold.AddBaseValue(slot.Item.SellCost);
                RealInventory.Remove(slot.Item);
                trash.PlaySound();
            }
            else
            {
                var inventory = result.gameObject.GetComponentInParent<UIInventory>();
                if (inventory != null && this != inventory && !inventory.IsFull &&
                    (!inventory.IsActiveInventory || !slot.Item.Unique || !inventory.RealInventory.ContainsByType(slot.Item.Type)))
                {
                    inventory.RealInventory.Add(slot.Item);
                    RealInventory.Remove(slot.Item);
                }
            }
        }
        slot.ImageTransform.SetParent(slot.transform);
        slot.ImageTransform.localPosition = Vector3.zero;
        _draging = false;
        _dragingItem = null;
    }

    private void OnBeginDrag(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty)
            return;
        _dragingItem = slot;
        TooltipController.Instance.UnassignTooltip();
        _draging = true;
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

    public void HighlightItems(ItemType type)
    {
        var validComponents = Recipes.GetAllSecondComponents(type);
        foreach (var component in validComponents)
        {
            foreach (var slot in _slots)
            {
                if (slot.Item == null || type == ItemType.None || slot.Item.Type == component)
                {
                    slot.ToLight();
                }
                else
                {
                    slot.ToDark();
                }
            }
        }
    }
}
