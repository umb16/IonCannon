using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class CooldownIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _frontImage;
    [SerializeField] private Image _iconImage;
    private CooldownsPanel _panel;

    public async UniTask Init(AddressKeys address, CooldownsPanel panel)
    {
        _panel = panel;
        _iconImage.sprite = await Addressables.LoadAssetAsync<Sprite>(AddressKeysConverter.Convert(address)).Task;
    }
    //[EditorButton]
    public void SetTime(float time, float maxTime)
    {
        _frontImage.fillAmount = time/maxTime;
        /*if(time < 1)
            _text.text = time.ToString(".00");
        else*/
            _text.text = time.ToString("#.");
    }

    public void Destroy()
    {
        _panel.RemoveIndicator(this);
    }
}
