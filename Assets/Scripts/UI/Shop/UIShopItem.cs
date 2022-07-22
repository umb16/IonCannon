using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
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
        SetText();
        _image.sprite = await Addressables.LoadAssetAsync<Sprite>(Item.Icon).Task;
        _image.gameObject.SetActive(true);
        //CheckButtonStatus(_player.Value.Gold);
        CheckButtonStatus(_player.Value.Gold);
    }

    private void SetText()
    {
        if (Item == null)
            return;
        _nameText.text = Item.Name;
        if (Item.Unique)
            _text.text = "<color=red>" + LocaleKeys.Main.UNIQUE.GetLocalizedString() + "</color>\n" + Item.Description;
        else
            _text.text = Item.Description;
    }

    private void CheckButtonStatus(ComplexStat stat)
    {
        if (stat.Value >= Item.Cost)
        {
            _buyButton.interactable = true;
            _buyButtonText.text = Item.Cost.ToString();
        }
        else
        {
            _buyButton.interactable = false;
            _buyButtonText.text = "<color=red>" + Item.Cost.ToString() + "</color>";
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform.parent);
    }

    //private void SetText(Locale x) => SetText();
    private void CheckButtonStatus(Locale x)
    {
        if (_player?.Value != null)
            CheckButtonStatus(_player.Value.Gold);
    }

    private void OnEnable()
    {
        //LocalizationSettings.SelectedLocaleChanged += SetText;
        LocalizationSettings.SelectedLocaleChanged += CheckButtonStatus;
    }

    private void OnDisable()
    {
        //LocalizationSettings.SelectedLocaleChanged -= SetText;
        LocalizationSettings.SelectedLocaleChanged -= CheckButtonStatus;
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
