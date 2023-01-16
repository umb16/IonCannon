using MiniScriptSharp;
using NewCheatPanel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class CheatPanelLayer : MonoBehaviour
{
    public static bool Enabled;
    [SerializeField] CheatPanelList _categories;
    [SerializeField] CheatPanelList _cheats;
    [SerializeField] NewCategoryMsgBox _msgBox;
    [SerializeField] CodeEditorBox _codeBox;
    [SerializeField] TMP_Text _categoryText;

    private PanelData _panelData;
    private Category _currentCategory;
    private bool _timeStoped;

    public Interpreter interpreter = new Interpreter();
    private string _dataPath = "CheatPanelData";
    private string _saveName = "Save";
    private string SaveDirectoryPath => Application.dataPath + $"/{_dataPath}/";
    private string SavePath => SaveDirectoryPath + _saveName + ".json";

    JsonSerializer _serializer = new JsonSerializer();

    Dictionary<KeyCode, List<Action>> _binds;

    private void OnEnable()
    {
        if (Time.timeScale < 1)
        {
            _timeStoped = true;
        }
        else
            Time.timeScale = 0;
        Enabled = true;
    }
    private void OnDisable()
    {
        if (!_timeStoped)
            Time.timeScale = 1;
        Enabled = false;
    }

    public void AddCategory(string name)
    {
        _panelData.TryAddCategory(name);
        if (_currentCategory == null)
            SetCurrentCategory(name);
        Save();
        UpdateUI();
    }

    private IEnumerable<KeyCode> StringBindsToEnumerable(string binds)
    {
        var split = binds.Split(' ');
        if(split.Length == 0)
            return Enumerable.Empty<KeyCode>();
        return split.Select(x =>
                     {
                         if (Enum.TryParse<KeyCode>(x, true, out var code))
                             return code;
                         return KeyCode.None;
                     })
                     .Where(x => x != KeyCode.None);
    }

    public void TryAddCheat(string name, string code, string binds)
    {
        if (_currentCategory == null)
            return;

        _currentCategory.AddCheat(name, code, StringBindsToEnumerable(binds));
        Save();
        UpdateUI();
    }

    public void HideCheat(string name)
    {
        if (_currentCategory == null)
            return;
        _currentCategory.Cheats[name].Hided = true;
        Save();
        UpdateUI();
    }

    public void HideCategory(string name)
    {
        _panelData.Categories[name].Hided = true;
        if (_currentCategory != null && _currentCategory.Name == name)
            _currentCategory = null;
        Save();
        UpdateUI();
    }

    public void SetCurrentCategory(string name)
    {
        _currentCategory = _panelData.Categories[name];
        _categoryText.text = _currentCategory.Name;
        UpdateUI();
    }

    public void OpenCheat(string name)
    {
        _codeBox.Show(name, TryAddCheat, _currentCategory.Cheats[name].Code, _currentCategory.Cheats[name].Binds);
    }

    public void CreateCategoryMsgBox()
    {
        _msgBox.Show("", AddCategory);
    }

    public void CreateCheatBox()
    {
        if (_currentCategory == null)
            return;
        _msgBox.Show("", (x) => TryAddCheat(x, "", ""));
    }
    private void Execute(string name)
    {
        interpreter.Reset(_currentCategory.Cheats[name].Code);
        interpreter.Compile();
    }
    private void Execute(string category, string cheat)
    {
        interpreter.Reset(_panelData.Categories[category].Cheats[cheat].Code);
        interpreter.Compile();
    }


    private void Awake()
    {
        if (!File.Exists(SavePath))
        {
            if (!Directory.Exists(SaveDirectoryPath))
                Directory.CreateDirectory(SaveDirectoryPath);

            using (FileStream fs = File.Create(SavePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("");
                fs.Write(info, 0, info.Length);
            }
            Save();
        }

        try
        {
            using (StreamReader sr = new StreamReader(SavePath))
            using (JsonReader reader = new JsonTextReader(sr))
                _panelData = _serializer.Deserialize<PanelData>(reader);
        }
        catch
        {
            Debug.LogWarning(SavePath + " corrupted");
            _panelData = null;
        }
        if (_panelData == null)
        {
            _panelData = new PanelData();
            Save();
        }
        interpreter.StandardOutput = Debug.Log;
        interpreter.ImplicitOutput = null;
        interpreter.ErrorOutput = (string s) =>
        {
            Debug.LogWarning(s);
            //target.interpreter.Stop();
        };
        _categories.SetMethods(SetCurrentCategory, x => { _msgBox.Show(x, AddCategory); }, HideCategory);
        _cheats.SetMethods(Execute, OpenCheat, HideCheat);
        UpdateUI();
        UniTaskAsyncEnumerable.EveryUpdate().Subscribe(_ =>
        {
            UpdateX();
        });
    }

    private void Save()
    {
        using (StreamWriter sw = new StreamWriter(SavePath))
        using (JsonWriter writer = new JsonTextWriter(sw))
            _serializer.Serialize(writer, _panelData ?? new PanelData());
    }

    public void UpdateUI()
    {
        _categories.UpdateElements(_panelData.Categories.Where(x => !x.Value.Hided).Select(x => x.Key));
        if (_currentCategory == null)
            _cheats.UpdateElements(new string[0]);
        else
            _cheats.UpdateElements(_currentCategory.Cheats.Where(x => !x.Value.Hided).Select(x => x.Key));
        _binds = _panelData.GetAllBinds(Execute);
    }

    void UpdateX()
    {
        if (interpreter.Running())
        {
            interpreter.RunUntilDone(0.01);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
        foreach (var bind in _binds)
        {
            if (Input.GetKeyDown(bind.Key))
            {
                foreach (var action in bind.Value)
                {
                    action();
                }
            }
        }
    }
}
