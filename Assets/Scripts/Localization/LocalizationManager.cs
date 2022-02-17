using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class LocalizationManager
{
    public static LocalizationManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LocalizationManager();
            return _instance;
        }
    }
    private static LocalizationManager _instance;
    private Subscriber LocalizationChanged = new Subscriber();
    private string defaultLang = "ru";
    private Dictionary<string, Dictionary<string, string>> _allLocals = new Dictionary<string, Dictionary<string, string>>();
    private Dictionary<string, string> _currentLocalDict;
    private Dictionary<string, string> _defaultLocalDict;
    public string[] AvaliableLangs => _allLocals.Keys.ToArray();

    public LocalizationManager(string lang = "ru")
    {
        var assets = Resources.LoadAll<TextAsset>("LocalizationFiles");
        foreach (var textAsset in assets)
        {
            _allLocals.Add(new CultureInfo(textAsset.name.Split("-")[0]).NativeName, JavaLangStrings.ConverJavaLocalToDict(textAsset.text));
        }
        _currentLocalDict = _allLocals[new CultureInfo(lang).NativeName];
        _defaultLocalDict = _allLocals[new CultureInfo(defaultLang).NativeName];
    }

    public void SetLang(string langName)
    {
        _currentLocalDict = _allLocals[langName];
        OnLocalizationChanged();
    }

    public void SubscribeOnChanges(Action action, Func<bool> usubscribeCondition)
    {
        LocalizationChanged.Add(action, usubscribeCondition);
    }

    public string GetPhrase(LocKeys key)
    {
        if (_currentLocalDict == null)
            return "###localization dict is null###";
        if (_currentLocalDict.TryGetValue(LocKeyConverter.Convert(key), out string value))
            return value;
        if (_defaultLocalDict.TryGetValue(LocKeyConverter.Convert(key), out string value2))
            return value2;
        return "###localization string not found###";
    }

    private void OnLocalizationChanged()
    {
        LocalizationChanged.Invoke();
    }
}
