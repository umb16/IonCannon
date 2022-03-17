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
    private List<StatModificator> _transformChain = new List<StatModificator>();
    private List<StatModificator> _correction = new List<StatModificator>();

    public float Value => _cachedValue;
    public int IntValue => _intCachedValue;
    public float BaseValue => _isUsedFunc ? _baseValueFunc() : _baseValue;
    public float Ratio => Value / BaseValue;

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

    public void AddBaseValue(float value)
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
        switch (modificator.Type)
        {
            case StatModificatorType.Additive:
                _additiveModificators.Add(modificator);
                break;
            case StatModificatorType.Multiplicative:
                _multiplicativeModificators.Add(modificator);
                break;
            case StatModificatorType.TransformChain:
                _transformChain.Add(modificator);
                break;
            case StatModificatorType.Correction:
                _correction.Add(modificator);
                break;
            default:
                break;
        }
        CalculateCache();
    }

    public void RemoveModificator(StatModificator mod)
    {
        switch (mod.Type)
        {
            case StatModificatorType.Additive:
                _additiveModificators.Remove(mod);
                break;
            case StatModificatorType.Multiplicative:
                _multiplicativeModificators.Remove(mod);
                break;
            case StatModificatorType.TransformChain:
                _transformChain.Remove(mod);
                break;
            case StatModificatorType.Correction:
                _correction.Remove(mod);
                break;
            default:
                break;
        }
        CalculateCache();
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

        float transformAccum = BaseValue;
        for (int i = 0; i < _transformChain.Count; i++)
        {
            transformAccum = _transformChain[i].Func(transformAccum);
        }

        _cachedValue = (transformAccum + additiveSum) * (1f + multSum);
        if (_correctionFunc != null)
            _cachedValue = _correctionFunc.Invoke(_cachedValue);
        for (int i = 0; i < _correction.Count; i++)
        {
            _cachedValue = _correction[i].Func(_cachedValue);
        }
        _intCachedValue = (int)_cachedValue;
        OnValueChanged();
    }
}