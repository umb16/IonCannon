using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using Button = UnityEngine.UI.Button;

public class UIShopLayer : BaseLayer
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _itemsRoot;
    [SerializeField] private Button _refrashButton;
    public event Action OnClosed;
    private int _refrashCount = 0;
    private int _itemsCount = 4;
    public List<UIShopItem> _items = new List<UIShopItem>();
    private AsyncReactiveProperty<Player> _player;
    private UIPlayerInventory _playerInventory;
    private GameData _gameData;
    private UIPlayerStats _playerStats;
    private ItemsDB _itemsDB;

    public bool Lock { get; private set; } = false;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player, GameData gameData,ItemsDB itemsDB)
    {
        _player = player;
        _gameData = gameData;
        _itemsDB = itemsDB;
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

    private async UniTask OnEnable()
    {
        _playerInventory = Show<UIPlayerInventory>();
        _playerStats = Show<UIPlayerStats>();
        Time.timeScale = 0;
        await UniTask.WaitUntil(()=>_gameData != null);   
        _gameData.State = GameState.InShop;
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
            _items[i].Set(_itemsDB.GetRandomItem()).Forget();
        }
    }

    private void OnBuyButtonClicked(UIShopItem item)
    {
        if (_player.Value.AddItemDirectly(item.Item))
        {
            item.gameObject.SetActive(false);
            _player.Value.Gold.AddBaseValue(-item.Item.Cost);
            _playerInventory.HighlightItems(ItemType.None);
        }
        else
            BaseLayer.Show<MsgBox>().Set("Нехватает места");
            //MessageBox.Show("Нехватает места");
    }

    public void Close()
    {
        OnClosed?.Invoke();
        Time.timeScale = 1;
        Hide();
        _playerInventory.Hide();
        _playerStats.Hide();
        new Timer(.1f).SetEnd(() => _gameData.State = GameState.Gameplay);
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
