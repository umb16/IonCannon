using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownsPanel : MonoBehaviour
{
    [SerializeField] private GameObject _cooldownIndicatorPrefab;
    private List<CooldownIndicator> _cooldowns = new List<CooldownIndicator>();
    public CooldownIndicator AddIndiacator(AddressKeys address)
    {
        var go = Instantiate(_cooldownIndicatorPrefab, transform);
        var indicator = go.GetComponent<CooldownIndicator>();
        indicator.Init(address, this).Forget();
        _cooldowns.Add(indicator);
        return indicator;
    }
    public void RemoveIndicator(CooldownIndicator indicator)
    {
        _cooldowns.Remove(indicator);
        if (indicator != null)
            Destroy(indicator.gameObject);
    }
}
