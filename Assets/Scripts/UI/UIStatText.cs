using TMPro;
using UnityEngine;

public class UIStatText : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_Text _value;
    public void SetText(string text) => _text.text = text;
    public void SetValue(string text) => _value.text = text;
}
