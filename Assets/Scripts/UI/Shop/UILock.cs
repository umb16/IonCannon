using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UILock : MonoBehaviour
{
    [SerializeField] private GameObject _lockImage;
    [SerializeField] private GameObject _unlockImage;
    private UIShopLayer _shop;

    [Inject]
    private void Awake()
    {
        _shop = BaseLayer.ForceGet<UIShopLayer>();
    }

    private void OnEnable()
    {
        _lockImage.SetActive(false);
        _unlockImage.SetActive(true);
    }

    public void OnPressLock()
    {
        if (_shop.Lock)
        {
            _lockImage.SetActive(false);
            _unlockImage.SetActive(true);
        }
        else
        {
            _lockImage.SetActive(true);
            _unlockImage.SetActive(false);
        }
        _shop.SetLock(!_shop.Lock);
    }
}
