using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PerksMenuElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _discription;
    [SerializeField] private Button _button;

    private Action Click;

    private void Awake()
    {
        _button.onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        Click?.Invoke();
    }

    public void Show(string name, string discription, Action action)
    {
        gameObject.SetActive(true);
        _name.text = name;
        _discription.text = discription;
        Click = action;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
