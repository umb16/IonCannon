using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UIStatText : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_Text _value;
    [SerializeField] Image _icon;
    public void SetText(string text) => _text.text = text;
    public void SetValue(string text) => _value.text = text;
    public async UniTask SetIconAsync(string iconAddress) => _icon.sprite = await Addressables.LoadAssetAsync<Sprite>(iconAddress).Task;
}
