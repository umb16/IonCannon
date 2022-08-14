using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LobbyUI : BaseLayer
{
    [SerializeField] private CharCell[] _chars;
    private string[] _charsNames = { Addresses.Char_standart, Addresses.Char_T_300 };
    private string _charName;
    private GameData _gameData;

    [Inject]
    private void Construct(GameData gameData)
    {
        _gameData = gameData;
    }
    private void OnEnable()
    {
        CharSelect(0);
    }

    public void StartGame()
    {
        _gameData.StartGame(_charName).ContinueWith(() =>
        {
            Show<InGameHUDLayer>();
        }).Forget();
        Hide();
    }

    public void CharSelect(int index)
    {
        Debug.Log(index);
        for (int i = 0; i < _chars.Length; i++)
        {
            var charCell = _chars[i];
            if (i == index)
            {
                charCell.SetSelected(true);
                _charName = _charsNames[i];
            }
            else
                charCell.SetSelected(false);
        }
    }

}
