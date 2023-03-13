using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Zenject;

public class DamageNumbersGenerator : MonoBehaviour
{
    [SerializeField] private DamageNumber numberPrefab;
    private DamageController _damageController;

    [Inject]
    private void Construct(DamageController damageController, GameData gameData)
    {
        _damageController = damageController;
        _damageController.Damage += CreateNumber;
        gameData.OnReset += OnGameReset;
    }

    private void OnGameReset()
    {
        foreach (var trn in GetComponentsInChildren<Transform>().Skip(1))
        {
            Destroy(trn.gameObject);
        }
    }

    private void CreateNumber(DamageMessage msg)
    {
        if (msg.Target is IMob target)
        {
            if (target.Type == MobType.Liquid)
                return;
            // if (msg.Target is Player && msg.DamageSource != DamageSources.Heal)
            //    return;
            var number = Instantiate(numberPrefab);
            number.transform.SetParent(transform);
            number.transform.position = target.Position - Vector3.up * .1f;

            if (msg.DamageSource == DamageSources.Heal)
                number.SetText("<color=green>" + (-msg.Damage).ToString("0.#", new CultureInfo("en-US", false)) + "</color>");
            else if (msg.Target is Player)
            {
                number.SetText("<color=red>" + msg.Damage.ToString("0.#", new CultureInfo("en-US", false)) + "</color>");
            }
            else if (msg.DamageSource == DamageSources.Ionization)
            {
                number.SetText("<color=#FFCF48>" + msg.Damage.ToString("0.#", new CultureInfo("en-US", false)) + "</color>");
            }
            else
                number.SetText(msg.Damage.ToString("0.#", new CultureInfo("en-US", false)));
        }
    }

    private void OnDestroy()
    {
        _damageController.Damage -= CreateNumber;
    }
}
