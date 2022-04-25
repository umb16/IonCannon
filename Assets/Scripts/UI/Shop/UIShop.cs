using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UIShop : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _itemsRoot;
    public event Action OnClosed;
    private int _refrashCount = 0;
    private int _itemsCount = 4;
    public List<UIShopItem> _items = new List<UIShopItem>();
    private Player _player;
    private UIPlayerInventory _playerInventory;

    public bool Lock { get; private set; } = false;

    [Inject]
    private void Construct(Player player, UIPlayerInventory playerInventory)
    {
        _player = player;
        _playerInventory = playerInventory;
    }

    private void Awake()
    {
        for (int i = 0; i < _itemsCount; i++)
        {
            var item = _items[i];
            item.OnBuyButtonClicked += OnBuyButtonClicked;
            item.PointerEnter += x => OnItemPointerEnter(x, item);
            item.PointerExit += OnItemPointerExit;
        }
    }

    private void OnItemPointerExit(PointerEventData obj)
    {
        _playerInventory.HighlightItems(ItemType.None);
    }

    private void OnItemPointerEnter(PointerEventData obj, UIShopItem shopItem)
    {
        var item = shopItem.Item;
        if (item != null)
            _playerInventory.HighlightItems(item.Type);
    }

    private void OnEnable()
    {
        if (!Lock)
            Generate();
        else
            Lock = false;
    }

    private void Generate()
    {
        for (int i = 0; i < _itemsCount; i++)
        {
            _items[i].gameObject.SetActive(true);
            _items[i].Set(ItemsDB.GetRandomItem()).Forget();
        }
    }

    private void OnBuyButtonClicked(UIShopItem item)
    {
        if (_player.AddItemDirectly(item.Item))
        {
            item.gameObject.SetActive(false);
            _player.Gold.AddBaseValue(-item.Item.Cost);
        }
        else
            MessageBox.Show("Нехватает места");
    }

    public void OnClose()
    {
        OnClosed?.Invoke();
    }

    public void Refrash()
    {
        _refrashCount++;
        Generate();
    }

    public void SetLock(bool value)
    {
        Lock = value;
    }
}
