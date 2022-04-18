using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventory : MonoBehaviour
{
    public event Action<Item> OnAdded;
    public event Action<Item> OnRemoved;
    [SerializeField] private GameObject _uiSlotPrefab;
    private List<UIInventorySlot> _slots = new List<UIInventorySlot>();
    private static bool _draging;
    private void Start()
    {
        SetSlotsCount(6);
        AddItem(new Item());
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
            _slots.Add(slot);
        }
    }

    private void OnPointerExit(PointerEventData eventData)
    {
        TooltipController.Instance.UnassignTooltip();
    }

    private void OnPointerEnter(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty || _draging)
            return;
        TooltipController.Instance.
            AssignTooltip(@$"<color=yellow><size=30>{slot.Item.Name}</size></color>
<color=red>”никальное</color>
{slot.Item.Description}");
    }



    private void OnEndDrag(PointerEventData eventData, UIInventorySlot slot)
    {
        _draging = false;
        if (slot.IsEmpty)
            return;
        var result = eventData.pointerCurrentRaycast;
        if (result.gameObject.GetComponentInParent<UIInventoryTrashCan>() != null)
        {
            RemoveItem(slot.Item);
        }
        else
        {
            var inventory = result.gameObject.GetComponentInParent<UIInventory>();
            if (inventory != null && this != inventory)
            {
                inventory.AddItem(slot.Item);
                RemoveItem(slot.Item);
            }
        }
        slot.ImageTransform.SetParent(slot.transform);
        slot.ImageTransform.localPosition = Vector3.zero;
        Debug.Log("OnEndDrag " + result.gameObject.name);
    }

    private void OnBeginDrag(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty)
            return;
        TooltipController.Instance.UnassignTooltip();
        _draging = true;
        slot.ImageTransform.SetParent(transform.parent, true);
    }
    private void OnDrag(PointerEventData eventData, UIInventorySlot slot)
    {
        if (slot.IsEmpty)
            return;
        var result = eventData.pointerCurrentRaycast;
        if (result.gameObject != null)
            Debug.Log(result.gameObject.name);
        slot.ImageTransform.position = eventData.position;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].IsEmpty)
            {
                _slots[i].Set(item).Forget();
                OnAdded?.Invoke(item);
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
                OnRemoved?.Invoke(item);
                break;
            }
        }
    }
}
