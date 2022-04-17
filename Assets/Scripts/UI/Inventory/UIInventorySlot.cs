using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _costText;
    public UIInventory Inventory;
    public Item Item { get; private set; }
    public bool IsEmpty => Item == null;

    public void Clear()
    {
        Item = null;
        _image.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        Debug.Log("OnBeginDrag");
        //_image.transform.position = eventData.position;
        _image.transform.SetParent(Inventory.transform.parent, true);
        
        
        Debug.Log(eventData.position);
        //(.ForceUpdateRectTransforms();
       // LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_image.transform.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        _image.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsEmpty)
            return;
        var result = eventData.pointerCurrentRaycast;
        if (result.gameObject.GetComponentInParent<UIInventoryTrashCan>() != null)
        {
            Inventory.RemoveItem(Item);
        }
        else
        {
            var inventory = result.gameObject.GetComponentInParent<UIInventory>();
            if (inventory != null && Inventory != inventory)
            {
                inventory.AddItem(Item);
                Inventory.RemoveItem(Item);
            }
        }
        _image.transform.SetParent(transform);
        _image.transform.localPosition = Vector3.zero;
        Debug.Log("OnEndDrag "+ result.gameObject.name);
    }

    public async UniTask Set(Item item)
    {
        Item = item;
        _costText.text = item.Cost.ToString();
        _image.sprite = await Addressables.LoadAssetAsync<Sprite>(AddressKeysConverter.Convert(item.AddressKeys)).Task;
        _image.gameObject.SetActive(true);
    }
}
