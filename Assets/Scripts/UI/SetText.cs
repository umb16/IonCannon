using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    [SerializeField] LocKeysStableEnum _key;
    [SerializeField] string _pefix;
    [SerializeField] string _affix;

    private TMP_Text _text;
    private bool _destroyed;
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        Set();
        LocalizationManager.Instance.SubscribeOnChanges(Set, () => _destroyed);
    }

    private void Set()
    {
        _text.text = _pefix + LocalizationManager.Instance.GetPhrase(_key) + _affix;
    }

    private void OnDestroy()
    {
        _destroyed = true;
    }
}
