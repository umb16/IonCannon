using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    [SerializeField] LocKeysStableEnum _key;
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
        _text.text = LocalizationManager.Instance.GetPhrase(_key);
    }

    private void OnDestroy()
    {
        _destroyed = true;
    }
}
