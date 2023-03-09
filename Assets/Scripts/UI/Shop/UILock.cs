using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UILock : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Sprite _unlockSprite;
    private UIShopLayer _shop;

    [Inject]
    private void Construct(UIShopLayer shop)
    {
        _shop = shop;
    }
    private void Awake()
    {

    }

    private void OnEnable()
    {
        _image.sprite = _unlockSprite;
    }

    public void OnPressLock()
    {
        if (_shop.Lock)
        {
            _image.sprite = _unlockSprite;
        }
        else
        {
            _image.sprite = _lockSprite;
        }
        _shop.SetLock(!_shop.Lock);
    }
}
