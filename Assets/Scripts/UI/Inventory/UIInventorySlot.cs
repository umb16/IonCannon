using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] private Image _image;
    [SerializeField] private Image _frameImage;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private GameObject _upgradeLable;
    [SerializeField] private TMP_Text _upgradeText;
    [SerializeField] private Sprite _normal;
    [SerializeField] private Sprite _empty;
    [SerializeField] private Sprite _hightlighted;
    public event Action<PointerEventData> BeginDrag;
    public event Action<PointerEventData> Drag;
    public event Action<PointerEventData> EndDrag;
    public event Action<PointerEventData> PointerEnter;
    public event Action<PointerEventData> PointerExit;
    public Action<Item> RemoveFromInventoryAction;
    public Action<Item> AddToInventoryAction;
    public Item Item { get; private set; }
    public Transform ImageTransform => _image.transform;
    public bool IsEmpty => Item == null;
    public void Clear()
    {
        Item = null;
        _image.gameObject.SetActive(false);
        Empty();
    }

    public void Normal()
    {
        _frameImage.sprite = _normal;
    }
    public void Highlighted()
    {
        _frameImage.sprite = _hightlighted;
    }
    public void Empty()
    {
        _frameImage.sprite = _empty;
    }
    public void OnBeginDrag(PointerEventData eventData) => BeginDrag?.Invoke(eventData);

    public void OnDrag(PointerEventData eventData) => Drag.Invoke(eventData);

    public void OnEndDrag(PointerEventData eventData) => EndDrag.Invoke(eventData);

    public void RemoveFromInventory() => RemoveFromInventoryAction(Item);
    public void AddToInventory(Item item) => AddToInventoryAction(item);

    public async UniTask Set(Item item)
    {
        Item = item;
        Normal();
        _costText.text = item.SellCost.ToString();
        _image.sprite = await Addressables.LoadAssetAsync<Sprite>(item.Icon).Task;
        _image.SetNativeSize();
        _image.gameObject.SetActive(true);
        if (item.UpgradeCount > 0)
        {
            _upgradeLable.SetActive(true);
            _upgradeText.text = new string('+', item.UpgradeCount);
        }
        else
        {
            _upgradeLable.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => PointerEnter?.Invoke(eventData);

    public void OnPointerExit(PointerEventData eventData) => PointerExit?.Invoke(eventData);
}
