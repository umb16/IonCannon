using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIElement
{
    event Action OnFinishHiding;
    void Hide();
}
