using System.Collections;
using System.Collections.Generic;
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
        gameData.GameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Restart)
        {
            foreach (var trn in GetComponentsInChildren<Transform>().Skip(1))
            {
                Destroy(trn.gameObject);
            }
        }
    }

    private void CreateNumber(DamageMessage msg)
    {
        if (msg.Target.Type == MobType.Liquid)
            return;
        if (msg.Target is Player && msg.DamageSource != DamageSources.Heal)
            return;
        var number = Instantiate(numberPrefab);
        number.transform.SetParent(transform);
        number.transform.position = msg.Target.Position + Vector3.up - Vector3.forward * 1;

        if (msg.DamageSource == DamageSources.Heal)
            number.SetText("<color=green>" + (-msg.Damage).ToString("0.#") + "</color>");
        else if (msg.DamageSource == DamageSources.Ionization)
        {
            number.SetText("<color=#FFCF48>" + msg.Damage.ToString("0.#") + "</color>");
        }
        else
            number.SetText(msg.Damage.ToString("0.#"));
    }

    private void OnDestroy()
    {
        _damageController.Damage -= CreateNumber;
    }
}
