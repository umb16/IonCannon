using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatPanelList : MonoBehaviour
{
    [SerializeField] private CheatPanelElement _prefab;
    private List<CheatPanelElement> _list = new List<CheatPanelElement>();
    private Action<string> _execute;
    private Action<string> _edit;
    private Action<string> _hide;

    public void SetMethods(Action<string> execute, Action<string> edit, Action<string> hide)
    {
        _execute = execute;
        _edit = edit;
        _hide = hide;
    }
    public void UpdateElements(IEnumerable<string> elements)
    {
        foreach (CheatPanelElement elem in _list)
        {
            Destroy(elem.gameObject);
        }
        _list.Clear();

        foreach (var item in elements)
        {
            var element = Instantiate(_prefab, _prefab.transform.parent);
            element.gameObject.SetActive(true);
            _list.Add(element);
            element.SetName(item);
            element.Delete += _hide;
            element.Execute += _execute;
            element.Edit += _edit;
        }
        
    }
}
