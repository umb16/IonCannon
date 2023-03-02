using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MsgBox : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _buttonOk;
    [SerializeField] private Button _buttonCancel;

    private Action OkPressed;
    private Action CancelPressed;
    public void Set(string text, Action ok = null, Action cancel = null)
    {
        _text.text = text;
        if (cancel == null)
            _buttonCancel?.gameObject.SetActive(false);
        else
            _buttonCancel?.gameObject.SetActive(true);
        OkPressed = ok;
        CancelPressed = cancel;
    }

    private void Awake()
    {
        _buttonOk.onClick.AddListener(Ok);
        _buttonCancel?.onClick.AddListener(Cancel);
    }

    private void Cancel()
    {
        CancelPressed?.Invoke();
    }

    private void Ok()
    {
        OkPressed?.Invoke();
    }
}
