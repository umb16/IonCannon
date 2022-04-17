using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public event Action<Item> OnAdded;
    public event Action<Item> OnRemoved;
    [SerializeField] private GameObject _uiSlotPrefab;
    private List<UIInventorySlot> _slots = new List<UIInventorySlot>();

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
            _slots.Add(slot);
            slot.Inventory = this;
        }
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
