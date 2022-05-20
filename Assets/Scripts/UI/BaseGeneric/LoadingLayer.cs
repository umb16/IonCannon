using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingLayer : BaseLayer
{
    public void StartShowing(Action actionDelegate)
    {
        //Здесь можно замутить какую-нибудь анимацию, а потом выполнить загрузку
        actionDelegate?.Invoke();
    }
}
