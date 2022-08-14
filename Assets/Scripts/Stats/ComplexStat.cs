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
    private bool _baseLimitsOn;
    private float _baseLimitMax;
    private float _baseLimitMin;
    public event Action<ComplexStat> ValueChanged;

    private float _cachedValue;
    private int _intCachedValue;
    private List<IStatModificator> _additiveModificators = new List<IStatModificator>();
    private List<IStatModificator> _multiplicativeModificators = new List<IStatModificator>();
    private List<IStatModificator> _transformChain = new List<IStatModificator>();
    private List<IStatModificator> _correction = new List<IStatModificator>();

    public float ModValue => _cachedValue - BaseValue;
    public float Value => _cachedValue;
    public int IntValue => _intCachedValue;
    public float BaseValue => _isUsedFunc ? _baseValueFunc() : _baseValue;
    public float Ratio => BaseValue == 0 ? 1 : Value / BaseValue;
    public int Percents => Mathf.RoundToInt(Ratio * 100 - 100);

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

    public ComplexStat SetBaseLimit(float min, float max)
    {
        _baseLimitsOn = true;
        _baseLimitMin = min;
        _baseLimitMax = max;
        return this;
    }

    public void AddBaseValue(float value)
    {
        _baseValue += value;
        if (_baseLimitsOn)
            _baseValue = Mathf.Min(_baseLimitMax, Mathf.Max(_baseLimitMin, _baseValue));
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

    public void AddModificator(IStatModificator modificator)
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

    public void RemoveModificator(IStatModificator mod)
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