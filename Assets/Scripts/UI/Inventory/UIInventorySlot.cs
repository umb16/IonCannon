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
    [SerializeField] private TMP_Text _costText;
    public event Action<PointerEventData> BeginDrag;
    public event Action<PointerEventData> Drag;
    public event Action<PointerEventData> EndDrag;
    public event Action<PointerEventData> PointerEnter;
    public event Action<PointerEventData> PointerExit;
    public Item Item { get; private set; }
    public Transform ImageTransform => _image.transform;
    public bool IsEmpty => Item == null;
    private static bool _draging;
    public void Clear()
    {
        Item = null;
        _image.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData) => BeginDrag?.Invoke(eventData);

    public void OnDrag(PointerEventData eventData) => Drag.Invoke(eventData);

    public void OnEndDrag(PointerEventData eventData) => EndDrag.Invoke(eventData);

    public async UniTask Set(Item item)
    {
        Item = item;
        _costText.text = item.Cost.ToString();
        _image.sprite = await Addressables.LoadAssetAsync<Sprite>(AddressKeysConverter.Convert(item.AddressKeys)).Task;
        _image.gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData) => PointerEnter?.Invoke(eventData);

    public void OnPointerExit(PointerEventData eventData) => PointerExit?.Invoke(eventData);
}
