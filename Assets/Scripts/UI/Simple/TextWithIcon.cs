using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextWithIcon : MonoBehaviour
{
    [SerializeField] private TMP_Text _text; 
    public void SetText(string text)
    {
        _text.text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
