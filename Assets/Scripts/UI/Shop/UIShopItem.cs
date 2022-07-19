using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
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

    private AsyncReactiveProperty<Player> _player;

    public event Action<UIShopItem> OnBuyButtonClicked;
    public Item Item { get; private set; }

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;
        _player.Where(x => x != null).ForEachAsync(x => x.Gold.ValueChanged += CheckButtonStatus);
    }
    public async UniTask Set(Item item)
    {
        Item = item;

        _nameText.text = item.Name;
        if (item.Unique)
            _text.text = "<color=red>" + LocaleKeys.Main.UNIQUE.GetLocalizedString() + "</color>\n" + item.Description;
        else
            _text.text = item.Description;
        _image.sprite = await Addressables.LoadAssetAsync<Sprite>(item.Icon).Task;
        _image.gameObject.SetActive(true);
        //CheckButtonStatus(_player.Value.Gold);
        LocaleKeys.Main.BUY.StringChanged += _ => CheckButtonStatus(_player.Value.Gold);
    }

    private void CheckButtonStatus(ComplexStat stat)
    {
        if (stat.Value >= Item.Cost)
        {
            _buyButton.interactable = true;
            _buyButtonText.text = LocaleKeys.Main.BUY.GetLocalizedString() + " " + Item.Cost.ToString();
        }
        else
        {
            _buyButton.interactable = false;
            _buyButtonText.text = LocaleKeys.Main.BUY.GetLocalizedString() + " <color=red>" + Item.Cost.ToString() + "</color>";
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

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.fullyExited) return;
        PointerExit?.Invoke(eventData);
    }
}
