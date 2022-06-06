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
        await UniTask.WaitUntil(()=>_panel.Value != null);
        return _panel.Value.AddIndiacator(icon);
    }
}
