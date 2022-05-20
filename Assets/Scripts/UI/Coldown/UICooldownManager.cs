using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICooldownsManager
{
    private AsyncReactiveProperty<CooldownsPanel> _panel;

    public UICooldownsManager(AsyncReactiveProperty<CooldownsPanel> panel)
    {
        _panel = panel;
    }

    internal async UniTask<CooldownIndicator> AddIndiacator(AddressKeys icon)
    {
        return (await _panel.WaitAsync()).AddIndiacator(icon);
    }
}
