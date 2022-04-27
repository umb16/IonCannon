using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UIShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _buyButtonText;
    [SerializeField] private Button _buyButton;

    public event Action<PointerEventData> PointerEnter;
    public event Action<PointerEventData> PointerExit;

    private Player _player;

    public event Action<UIShopItem> OnBuyButtonClicked;
    public Item Item { get; private set; }
    
    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }
    public async UniTask Set(Item item)
    {
        Item = item;
       
        _nameText.text = item.Name;
        if (item.Unique)
            _text.text = "<color=red>Уникально</color>\n" + item.Description;
        else
            _text.text = item.Description;
        _image.sprite = await Addressables.LoadAssetAsync<Sprite>(AddressKeysConverter.Convert(item.Icon)).Task;
        _image.gameObject.SetActive(true);
        CheckButtonStatus(_player.Gold);
        _player.Gold.ValueChanged += CheckButtonStatus;
    }

    private void CheckButtonStatus(ComplexStat stat)
    {
        if (stat.Value >= Item.Cost)
        {
            _buyButton.interactable = true;
            _buyButtonText.text = "Купить " + Item.Cost.ToString();
        }
        else
        {
            _buyButton.interactable = false;
            _buyButtonText.text = "Купить <color=red>" + Item.Cost.ToString()+"</color>";
        }
    }

    private void OnDisable()
    {
       // _player.Gold.ValueChanged -= CheckButtonStatus;
    }

    private void Awake()
    {
        _buyButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnBuyButtonClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData) => PointerEnter?.Invoke(eventData);

    public void OnPointerExit(PointerEventData eventData) => PointerExit?.Invoke(eventData);
}
