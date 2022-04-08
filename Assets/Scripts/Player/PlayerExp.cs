using System;
using UnityEngine;

public class PlayerExp
{
    public event Action LevelUp;
    public event Action ExpGained;
    public int CurrentLevel { get; private set; }
    public int Requed { get; private set; } = 40;
    public int Value { get; private set; }
    public int Cumulative { get; private set; }
    public float Normalized => Value / (float)Requed;
    public int ComboFactor { get; private set; }

    public float NormalizedComboTime => Mathf.Max(0, (_comboTime - Time.time) / _comboDelay);
    private float _comboTime = .01f;
    private float _comboDelay = 2;
    public void AddExp(int exp)
    {
        if (_comboTime < Time.time)
        {
            ComboFactor = 1;
        }
        _comboTime = Time.time + _comboDelay;
        //ComboFactor++;
        Cumulative += exp;
        Value += exp; //* (ComboFactor - 1);
        if (Value >= Requed)
        {
            Value -= Requed;
            Requed = (int)(Requed * 1.3f);
            CurrentLevel++;
            LevelUp?.Invoke();
        }
        ExpGained?.Invoke();
    }

}
