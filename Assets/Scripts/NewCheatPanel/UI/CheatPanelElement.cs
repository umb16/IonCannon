using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatPanelElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    public event Action<string> Delete;
    public event Action<string> Edit;
    public event Action<string> Execute;
    public string Name { get; private set; }

    public void SetName(string name)
    {
        Name = name;
        _text.text = name;
    }

    public void OnEditButton()
    {
        Edit?.Invoke(Name);
    }
    public void OnExecuteButton()
    {
        Execute?.Invoke(Name);
    }
    public void OnDeleteButton()
    {
        Delete?.Invoke(Name);
    }
}
