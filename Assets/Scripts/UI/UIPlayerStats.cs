using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
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
    private void Construct(AsyncReactiveProperty<Player> player, GameData gameData)
    {
        _player = player;

        AddString(LocaleKeys.Main.MAX_HP, StatType.MaxHP, Addresses.Ico_HP);
        AddString(LocaleKeys.Main.MOVE_SPEED, StatType.MovementSpeed, Addresses.Ico_MoveSpeed, LocaleKeys.Main.P_S);
        AddString(LocaleKeys.Main.PICKUP_RADIUS, StatType.PickupRadius, Addresses.Ico_PickupRadius, LocaleKeys.Main.P);
        AddString(LocaleKeys.Main.RAY_DAMAGE, StatType.RayDamage, Addresses.Ico_RayDamage);
        AddString(LocaleKeys.Main.RAY_SPEED, StatType.RaySpeed, Addresses.Ico_RaySpeed, LocaleKeys.Main.P_S);
        AddString(LocaleKeys.Main.Capacity, StatType.Capacity, Addresses.Ico_RayPath);
        AddString(LocaleKeys.Main.EnergyRegen, StatType.EnergyRegen, Addresses.Ico_AtomicBattery);
        AddString(LocaleKeys.Main.RAY_CHARGE_DELAY, StatType.RayDelay, Addresses.Ico_RayDelay, LocaleKeys.Main.S, modPositive: false);
        AddString(LocaleKeys.Main.RAY_WIDTH, StatType.RayDamageAreaRadius, Addresses.Ico_RayWidth, LocaleKeys.Main.P);
        AddString(LocaleKeys.Main.RAY_ERROR, StatType.RayError, Addresses.Ico_RayError, LocaleKeys.Main.P, modPositive: false);

        gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState obj)
    {
        if (obj == GameState.StartMenu)
            Hide();
    }

    private void AddString(LocalizedString text, StatType statType, string icon, LocalizedString postfix = null, bool modPositive = true)
    {
        var maxHPText = AddString();

        text.StringChanged += x => maxHPText.SetText(x);
        //maxHPText.SetIconAsync(icon).Forget();
        _player.Where(x => x != null).ForEachAsync(x =>
        {
            var stat = x.StatsCollection.GetStat(statType);
            if (postfix == null)
            {
                maxHPText.SetValue(stat.Value.ToString("0.#", new CultureInfo("en-US", false)) + GetModText(stat, modPositive));
                stat.ValueChanged += x => maxHPText.SetValue(stat.Value.ToString("0.#", new CultureInfo("en-US", false)) + " " + GetModText(stat, modPositive));
            }
            else
            {
                maxHPText.SetValue(stat.Value.ToString("0.#", new CultureInfo("en-US", false)) +
                    " <font=\"GothaProBla SDF\" material=\"GothaProBla Without outline SDF Material\">" + postfix.GetLocalizedString() + "</font> " + GetModText(stat, modPositive));
                stat.ValueChanged += x => maxHPText.SetValue(stat.Value.ToString("0.#", new CultureInfo("en-US", false)) +
                    " <font=\"GothaProBla SDF\" material=\"GothaProBla Without outline SDF Material\">" + postfix.GetLocalizedString() + "</font> " + GetModText(stat, modPositive));
                postfix.StringChanged += x => maxHPText.SetValue(stat.Value.ToString("0.#", new CultureInfo("en-US", false)) +
                    " <font=\"GothaProBla SDF\" material=\"GothaProBla Without outline SDF Material\">" + x + "</font> " + GetModText(stat, modPositive));
            }
        });
    }

    private static string GetModText(ComplexStat stat, bool positive)
    {
        string modValue = stat.Percents.ToString();
        //Debug.Log(stat.Ratio);
        if (stat.Percents == 0)
        {
            modValue = "";
        }
        else if (stat.Percents > 0 && positive || stat.Percents < 0 && !positive)
        {
            modValue = "<color=green>(+" + modValue + "%)</color>";
        }
        else
        {
            modValue = "<color=red>(" + modValue + "%)</color>";
        }

        return modValue;
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
