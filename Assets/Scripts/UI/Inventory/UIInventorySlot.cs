using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
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
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        _image.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
        _image.transform.localPosition = Vector3.zero;
        Debug.Log("OnEndDrag "+ result.gameObject.name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
    }

    public async UniTask Set(Item item)
    {
        Item = item;
        _image.sprite = await Addressables.LoadAssetAsync<Sprite>(AddressKeysConverter.Convert(item.AddressKeys)).Task;
        _image.gameObject.SetActive(true);
    }
}
