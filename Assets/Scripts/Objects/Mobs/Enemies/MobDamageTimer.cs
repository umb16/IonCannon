using UnityEngine;

public class MobDamageTimer
{
    private float _nextDamageTime = 0;
    public float Cooldown = 1;

    public bool Update()
    {
        if (_nextDamageTime < Time.time)
        {
            _nextDamageTime = Time.time + Cooldown;
            return true;
        }
        return false;
    }
}
