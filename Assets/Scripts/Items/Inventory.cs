using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    public int SlotsCount { get; private set; } = 3;
    public bool FreeSlotAvailable => SlotsCount - _items.Count > 0;
    public event Action<Item> ItemAdded;
    public event Action<Item> ItemRemoved;
    public event Action SlotAdded;
    private List<Item> _items = new List<Item>();

    /*public void SetSlots(int value)
    {
        SlotsCount = value;
    }*/
    public void AddSlot()
    {
        SlotsCount++;
        SlotAdded?.Invoke();
    }

    public bool TryGetItem(int index, out Item item)
    {
        if (_items.Count > index)
        {
            item = _items[index];
            return true;
        }
        else
        {
            item = null;
            return false;
        }
    }
    /*public Item Get(int index)
    {
        return _items[index];
    }*/
    public void Add(Item item)
    {
        if (FreeSlotAvailable)
        {
            _items.Add(item);
            ItemAdded?.Invoke(item);
        }
    }
    public void Remove(Item item)
    {
        _items.Remove(item);
        ItemRemoved?.Invoke(item);
    }

    public bool ContainsUnique(Item item)
    {
        if (!item.Unique)
            return false;
        return ContainsByType(item.Type);
    }

    public bool ContainsByType(ItemType type)
    {
        return _items.Any(x => x.Type == type);
    }
}
