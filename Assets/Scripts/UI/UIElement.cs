using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    public void Hide()
    {
        OnHided();
    }
    protected virtual void OnHided()
    {
    }
}
