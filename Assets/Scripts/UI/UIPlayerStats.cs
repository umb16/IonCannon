using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

public class UIPlayerStats : BaseLayer
{
    [SerializeField] GameObject _textPrefab;
    private AsyncReactiveProperty<Player> _player;

    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;

        AddString(LocaleKeys.Main.MAX_HP, StatType.MaxHP);
        AddString(LocaleKeys.Main.MOVE_SPEED, StatType.MovementSpeed, LocaleKeys.Main.P_S);
        AddString(LocaleKeys.Main.PICKUP_RADIUS, StatType.PickupRadius, LocaleKeys.Main.P);
        AddString(LocaleKeys.Main.RAY_DAMAGE, StatType.RayDamage);
        AddString(LocaleKeys.Main.RAY_SPEED, StatType.RaySpeed, LocaleKeys.Main.P_S);
        AddString(LocaleKeys.Main.RAY_PATH_LENGHT, StatType.RayPathLenght, LocaleKeys.Main.P);
        AddString(LocaleKeys.Main.RAY_CHARGE_DELAY, StatType.RayDelay, LocaleKeys.Main.S);
        AddString(LocaleKeys.Main.RAY_WIDTH, StatType.RayDamageAreaRadius, LocaleKeys.Main.P);
        AddString(LocaleKeys.Main.RAY_ERROR, StatType.RayError, LocaleKeys.Main.P);
    }

    private void AddString(string text, StatType statType, string postfix = "", string format = "")
    {
        var maxHPText = AddString();
        maxHPText.SetText(text);
        _player.Where(x => x != null).ForEachAsync(x =>
        {
            var stat = x.StatsCollection.GetStat(statType);
            maxHPText.SetValue(stat.Value.ToString(format) + postfix);
            stat.ValueChanged += x => maxHPText.SetValue(stat.Value.ToString(format) + postfix);
        });
    }
    private void AddString(LocalizedString text, StatType statType, LocalizedString postfix = null, string format = "")
    {
        var maxHPText = AddString();

        text.StringChanged += x => maxHPText.SetText(x);

        _player.Where(x => x != null).ForEachAsync(x =>
        {
            var stat = x.StatsCollection.GetStat(statType);
            maxHPText.SetValue(stat.Value.ToString(format, new CultureInfo("en-US", false)) + " " + postfix?.GetLocalizedString()??"");
            stat.ValueChanged += x => maxHPText.SetValue(stat.Value.ToString(format, new CultureInfo("en-US", false)) + " " + postfix?.GetLocalizedString() ?? "");
            if (postfix != null)
                postfix.StringChanged += x => maxHPText.SetValue(stat.Value.ToString(format, new CultureInfo("en-US", false)) + " " + x);
        });
    }
    private UIStatText AddString()
    {
        var go = Instantiate(_textPrefab, _textPrefab.transform.parent);
        go.SetActive(true);
        return go.GetComponent<UIStatText>();
    }
}
