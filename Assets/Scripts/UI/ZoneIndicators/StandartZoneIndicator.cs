using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;
[SerializeField]
public class StandartZoneIndicator : MonoBehaviour, IZoneIndicator
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] GameObject _icon;
    private float _radius = 3;
    private int _vertexCount = 50;
    private bool _needUpdate = true;
    private IDisposable _blinkTimer;

    private void OnEnabled()
    {

    }
    private void OnDisable()
    {
        _blinkTimer?.Dispose();
    }
    public void Hide()
    {
        Destroy(gameObject);
    }
    public void SetBlink(float value)
    {
        _blinkTimer?.Dispose();

        _lineRenderer.enabled = true;
        _icon.SetActive(true);
        if (value != 0)
        {
            StartBlink(value);
        }
    }
    private void StartBlink(float blinkTime)
    {
        _blinkTimer = UniTaskAsyncEnumerable.Timer(TimeSpan.FromSeconds(blinkTime), TimeSpan.FromSeconds(blinkTime)).Subscribe(_ =>
        {
            if (_lineRenderer.enabled)
            {
                _lineRenderer.enabled = false;
                _icon.SetActive(false);
            }
            else
            {
                _lineRenderer.enabled = true;
                _icon.SetActive(true);
            }
        });
    }

    public void SetIcon(string address)
    {
        throw new System.NotImplementedException();
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void SetRadius(float radius)
    {
        _needUpdate = true;
        _radius = radius;
    }

    private void Update()
    {
        if (_needUpdate)
        {
            _needUpdate = false;
            _lineRenderer.positionCount = _vertexCount;
            for (int i = 0; i < _vertexCount; i++)
            {
                Vector3 newPoint = Vector3.up.DiamondRotateXY(4.0f / _vertexCount * i);
                newPoint *= _radius;
                //newPoint += transform.position;
                //newPoint.z = 0;
                _lineRenderer.SetPosition(i, newPoint.Get2D(0));
            }
        }
    }
}
