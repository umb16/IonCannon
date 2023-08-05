using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using Button = UnityEngine.UI.Button;
using SPVD.LifeSupport;

public class UIShopLayer : MonoBehaviour
{
    [SerializeField] private Transform _itemsRoot;
    [SerializeField] private Button _refrashButton;
    public event Action OnClosed;
    private int _refrashCount = 0;
    private int _itemsCount = 4;
    public List<UIShopItem> _items = new List<UIShopItem>();
    private ShopShip _ship;
    private AsyncReactiveProperty<Player> _player;
    private UIPlayerInventory _playerInventory;
    private GameData _gameData;
    private UIPlayerStats _playerStats;
    private ItemsDB _itemsDB;

    public bool Lock { get; private set; } = false;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, GameData gameData,
        ItemsDB itemsDB, UIPlayerInventory uiPlayerInventory)
    {
        _player = player;
        _gameData = gameData;
        _itemsDB = itemsDB;
        _playerInventory = uiPlayerInventory;
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
        _playerInventory.HighlightItems(ItemId.None);
    }

    private void OnItemPointerEnter(PointerEventData obj, UIShopItem shopItem)
    {
        var item = shopItem.Item;
        if (item != null)
            _playerInventory.HighlightItems(item.Id);
    }

    private async UniTask OnEnable()
    {
        SoundManager.Instance.PlayShopOpen();
        await UniTask.WaitUntil(() => _gameData != null);
        _refrashButton.interactable = true;
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
            _items[i].Set(_itemsDB.CreateRandomItem()).Forget();
        }
    }

    private void OnBuyButtonClicked(UIShopItem item)
    {
        if (_player.Value.AddItemDirectly(item.Item))
        {
            item.gameObject.SetActive(false);
            _player.Value.Gold.AddBaseValue(-item.Item.Cost);
            _playerInventory.HighlightItems(ItemId.None);
        }
        else
        {
            //todo: Сообщение об ошибке (звук, мигание)
        }
        //BaseLayer.Show<MsgBox>().Set("Inventory is full");
        //MessageBox.Show("Не хватает места");
    }

    private void OnDisable()
    {
        
    }

    public void Close()
    {
        OnClosed?.Invoke();
        SoundManager.Instance.PlayShopClose();
        Time.timeScale = 1;
        _gameData.ReturnToPrevStatus();
    }

    public void Refrash()
    {
        _refrashCount++;
        _refrashButton.interactable = false;
        Generate();
    }

    public void SetLock(bool value)
    {
        Lock = value;
    }
}
