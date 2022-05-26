using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    public int SlotsCount { get; private set; } = 6;
    public bool FreeSlotAvailable => SlotsCount - _items.Count > 0;
    public event Action<Item> ItemAdded;
    public event Action<Item> ItemRemoved;
    private List<Item> _items = new List<Item>();

    public void SetSlots(int value)
    {
        SlotsCount = value;
    }

    public Item Get(int index)
    {
        return _items[index];
    }
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
