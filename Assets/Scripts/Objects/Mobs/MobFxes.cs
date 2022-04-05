using System.Collections.Generic;
using UnityEngine;

public class MobFxes
{
   private Dictionary<int, GameObject> _mobFxes = new Dictionary<int, GameObject>();
    public void Add(Fx fx, GameObject gameObject)
    {
        _mobFxes.Add(fx.Id, gameObject);
    }
    public void Remove(Fx fx)
    {
        if (_mobFxes.ContainsKey(fx.Id))
        {
            GameObject.Destroy(_mobFxes[fx.Id]);
            _mobFxes.Remove(fx.Id);
        }
    }
}
