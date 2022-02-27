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

    public float Value => _cachedValue;
    public int IntValue => _intCachedValue;

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
        if (modificator.Type == StatModificatorType.Additive)
        {
            _additiveModificators.Add(modificator);
        }
        else if(modificator.Type == StatModificatorType.Multiplicative)
        {
            _multiplicativeModificators.Add(modificator);
        }
        else
        {
            _transformChain.Add(modificator);
        }
        CalculateCache();
    }

    public void RemoveModificator(StatModificator mod)
    {
        if(mod.Type== StatModificatorType.Additive)
            _additiveModificators.Remove(mod);
        if(mod.Type== StatModificatorType.Multiplicative)
            _multiplicativeModificators.Remove(mod);
        if (mod.Type == StatModificatorType.TransformChain)
            _transformChain.Remove(mod);
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
        _intCachedValue = (int)_cachedValue;
        OnValueChanged();
    }
}