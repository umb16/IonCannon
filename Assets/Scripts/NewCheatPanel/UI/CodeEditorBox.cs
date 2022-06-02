using MiniscriptSharp.CodeEdit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeEditorBox : MonoBehaviour
{
    [SerializeField] private CodeEditor _codeEditor;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_InputField _keys;
    [SerializeField] Button _okButton;
    private Action<string, string, string> _okAction;

    private void Awake()
    {
        _inputField.onValueChanged.AddListener(CheckOkButton);
        _okButton.onClick.AddListener(OnOkButton);
    }

    private void CheckOkButton(string text)
    {
        if (string.IsNullOrEmpty(text))
            _okButton.gameObject.SetActive(false);
        else
            _okButton.gameObject.SetActive(true);
    }

    public void Show(string text, Action<string, string, string> action, string code, IEnumerable<KeyCode> binds)
    {
        gameObject.SetActive(true);
        _inputField.text = text;
        _codeEditor.source = code;
        _keys.text = string.Join(" ", binds);
        _okAction = action;
    }

    private void OnOkButton()
    {
        _okAction?.Invoke(_inputField.text, _codeEditor.source, _keys.text);
        gameObject.SetActive(false);
    }
}
