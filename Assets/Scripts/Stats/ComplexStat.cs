using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class ComplexStat
{
    [DataMember]
    private float _baseValue;

    private bool _isUsedFunc = false;
    private Func<float> _baseValueFunc = null;
    private Func<float, float> _correctionFunc = null;

    public event Action<ComplexStat> ValueChanged;

    private float _cachedValue;
    private int _intCachedValue;
    private List<StatModificator> _additiveModificators = new List<StatModificator>();
    private List<StatModificator> _multiplicativeModificators = new List<StatModificator>();

    public float Value => _cachedValue;
    public float IntValue => _intCachedValue;

    public float BaseValue => _isUsedFunc ? _baseValueFunc() : _baseValue;

    public ComplexStat(float newValue, Func<float, float> correctionFunc = null)
    {
        SetBaseValue(newValue);
        _correctionFunc = correctionFunc;
    }

    public ComplexStat(Func<float> baseValueFunc, Func<float, float> correctionFunc = null)
    {
        SetBaseValueFunc(baseValueFunc);
        _correctionFunc = correctionFunc;
    }

    public void AddBaseValue(int value)
    {
        _baseValue += value;
        if (_isUsedFunc)
        {
            Debug.LogError("Attention! You try use SetBaseValue() but baseValueFunc already setted");
        }
        else
            CalculateCache();
    }

    public void SetBaseValue(float newValue)
    {
        _baseValue = newValue;
        if (_isUsedFunc)
        {
            Debug.LogError("Attention! You try use SetBaseValue() but baseValueFunc already setted");
        }
        else
            CalculateCache();
    }

    public void SetParent(ComplexStat parentStat)
    {
        parentStat.ValueChanged += OnParentStatChanged;
    }

    public void AddModificator(StatModificator modificator)
    {
        if (modificator.Type == StatModificatorType.Additive)
        {
            _additiveModificators.Add(modificator);
        }
        else
        {
            _multiplicativeModificators.Add(modificator);
        }
        CalculateCache();
    }

    public void RemoveModificator(int modificatorId)
    {
        for (int i = 0; i < _additiveModificators.Count; i++)
        {
            if (_additiveModificators[i].Id == modificatorId)
            {
                _additiveModificators.RemoveAt(i);
                CalculateCache();
                return;
            }
        }

        for (int i = 0; i < _multiplicativeModificators.Count; i++)
        {
            if (_multiplicativeModificators[i].Id == modificatorId)
            {
                _multiplicativeModificators.RemoveAt(i);
                CalculateCache();
                return;
            }
        }
    }
    private void OnValueChanged()
    {
        ValueChanged?.Invoke(this);
    }

    private void SetBaseValueFunc(Func<float> baseValueFunc)
    {
        _baseValueFunc = baseValueFunc;
        _isUsedFunc = true;
        CalculateCache();
    }

    private void OnParentStatChanged(ComplexStat stat)
    {
        CalculateCache();
    }

    private void SetCorrectFunction(Func<float, float> correctFunction)
    {
        _correctionFunc = correctFunction;
    }

    private void CalculateCache()
    {
        float additiveSum = 0;
        for (int i = 0; i < _additiveModificators.Count; i++)
        {
            additiveSum += _additiveModificators[i].Value;
        }

        float multSum = 0;
        for (int i = 0; i < _multiplicativeModificators.Count; i++)
        {
            multSum += _multiplicativeModificators[i].Value;
        }

        _cachedValue = (BaseValue + additiveSum) * (1f + multSum);
        if (_correctionFunc != null)
            _cachedValue = _correctionFunc.Invoke(_cachedValue);
        _intCachedValue = (int)_cachedValue;
        OnValueChanged();
    }
}