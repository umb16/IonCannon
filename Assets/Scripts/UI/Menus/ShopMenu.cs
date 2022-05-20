using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : BaseMenu
{
    UIShopLayer _shopLayer;
    UIPlayerStats _statsLayer;
    UIPlayerInventory _inventoryLayer;

    protected override void OnShow()
    {
        _shopLayer = BaseLayer.Show<UIShopLayer>();
        _statsLayer = BaseLayer.Show<UIPlayerStats>();
        _inventoryLayer = BaseLayer.Show<UIPlayerInventory>();
    }
}
