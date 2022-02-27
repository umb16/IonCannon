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

    public void Show(string name, string discription, Action action)
    {
        gameObject.SetActive(true);
        _name.text = name;
        _discription.text = discription;
        _button.onClick.AddListener(() => action());
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
