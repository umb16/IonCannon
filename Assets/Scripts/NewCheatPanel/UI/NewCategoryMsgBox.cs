using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewCategoryMsgBox : MonoBehaviour
{
    [SerializeField] TMP_InputField _text;
    [SerializeField] Button _okButton;
    private Action<string> _okAction;

    private void Awake()
    {
        _text.onValueChanged.AddListener(CheckOkButton);
        _okButton.onClick.AddListener(OnOkButton);
    }

    private void OnOkButton()
    {
        _okAction?.Invoke(_text.text);
        gameObject.SetActive(false);
    }

    public void Show(string text, Action<string> action)
    {
        _text.text = text;
        gameObject.SetActive(true);
        _okAction = action;
    }

    private void CheckOkButton(string text)
    {
        if (string.IsNullOrEmpty(text))
            _okButton.gameObject.SetActive(false);
        else
            _okButton.gameObject.SetActive(true);
    }

}
