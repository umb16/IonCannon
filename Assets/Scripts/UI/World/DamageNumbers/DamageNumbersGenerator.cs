using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DamageNumbersGenerator : MonoBehaviour
{
    [SerializeField] private DamageNumber numberPrefab;
    private DamageController _damageController;

    [Inject]
    private void Construct(DamageController damageController)
    {
        _damageController = damageController;
        _damageController.Damage += CreateNumber;
    }

    private void CreateNumber(DamageMessage msg)
    {
        if (msg.Target is Player)
            return;
        var number = Instantiate(numberPrefab);
        number.transform.SetParent(transform);
        number.transform.position = msg.Target.Position + Vector3.up - Vector3.forward*1;
        
        number.SetText(msg.Damage.ToString("0.#"));
    }

    private void OnDestroy()
    {
        _damageController.Damage -= CreateNumber;
    }
}
