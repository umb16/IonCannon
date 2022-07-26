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

        AddString(LocaleKeys.Main.MAX_HP, StatType.MaxHP, Addresses.Ico_HP);
        AddString(LocaleKeys.Main.MOVE_SPEED, StatType.MovementSpeed, Addresses.Ico_MoveSpeed, LocaleKeys.Main.P_S);
        AddString(LocaleKeys.Main.PICKUP_RADIUS, StatType.PickupRadius, Addresses.Ico_PickupRadius, LocaleKeys.Main.P);
        AddString(LocaleKeys.Main.RAY_DAMAGE, StatType.RayDamage, Addresses.Ico_RayDamage);
        AddString(LocaleKeys.Main.RAY_SPEED, StatType.RaySpeed, Addresses.Ico_RaySpeed, LocaleKeys.Main.P_S);
        AddString(LocaleKeys.Main.RAY_PATH_LENGHT, StatType.RayPathLenght, Addresses.Ico_RayPath, LocaleKeys.Main.P);
        AddString(LocaleKeys.Main.RAY_CHARGE_DELAY, StatType.RayDelay, Addresses.Ico_RayDelay, LocaleKeys.Main.S);
        AddString(LocaleKeys.Main.RAY_WIDTH, StatType.RayDamageAreaRadius, Addresses.Ico_RayWidth, LocaleKeys.Main.P);
        AddString(LocaleKeys.Main.RAY_ERROR, StatType.RayError, Addresses.Ico_RayError, LocaleKeys.Main.P);
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
    private void AddString(LocalizedString text, StatType statType, string icon, LocalizedString postfix = null, string format = "")
    {
        var maxHPText = AddString();

        text.StringChanged += x => maxHPText.SetText(x);
        maxHPText.SetIconAsync(icon).Forget();
        _player.Where(x => x != null).ForEachAsync(x =>
        {
            var stat = x.StatsCollection.GetStat(statType);
            if (postfix == null)
            {
                maxHPText.SetValue(stat.Value.ToString(format, new CultureInfo("en-US", false)) + " ");
                stat.ValueChanged += x => maxHPText.SetValue(stat.Value.ToString(format, new CultureInfo("en-US", false)) + " ");
            }
            else
            {
                maxHPText.SetValue(stat.Value.ToString(format, new CultureInfo("en-US", false)) +
                    " <font=\"GothaProBla SDF\" material=\"GothaProBla Without outline SDF Material\">" + postfix.GetLocalizedString() + "</font>");
                stat.ValueChanged += x => maxHPText.SetValue(stat.Value.ToString(format, new CultureInfo("en-US", false)) +
                    " <font=\"GothaProBla SDF\" material=\"GothaProBla Without outline SDF Material\">" + postfix.GetLocalizedString() + "</font>");
                postfix.StringChanged += x => maxHPText.SetValue(stat.Value.ToString(format, new CultureInfo("en-US", false)) +
                    " <font=\"GothaProBla SDF\" material=\"GothaProBla Without outline SDF Material\">" + x + "</font>");
            }
        });
    }
    private UIStatText AddString()
    {
        var go = Instantiate(_textPrefab, _textPrefab.transform.parent);
        go.SetActive(true);
        return go.GetComponent<UIStatText>();
    }

    private void OnDisable()
    {
        BaseLayer.Hide<UIShopBack>();
    }
}
