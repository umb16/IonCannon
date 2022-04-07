using System.Collections;
using System.Collections.Generic;
using Umb16.Extensions;
using UnityEngine;
using Zenject;

public class LifeSupportTower : MonoBehaviour
{
    [SerializeField] float _radius = 30;
    [SerializeField] float _supportDrainSpeed = .25f;
    [SerializeField] LineRenderer _lineRenderer;
    private int _vertexCount = 100;
    private Player _player;
    ComplexStat _stat;


    [Inject]
    private void Construct(Player player)
    {
        _player = player;
        _stat = _player.StatsCollection.GetStat(StatType.LifeSupport);
        _lineRenderer.positionCount = _vertexCount;
        for (int i = 0; i < _vertexCount; i++)
        {
            Vector3 newPoint = Vector3.up.DiamondRotateXY(4.0f / _vertexCount * i);
            newPoint.y *= .8f;
            newPoint *= _radius;

            _lineRenderer.SetPosition(i, newPoint.Get2D());
        }
    }
    void Update()
    {
        if (_player == null)
            return;
        if (_player.Position.SqrMagnetudeXY() > _radius * _radius)
        {
            _stat.AddBaseValue(-_supportDrainSpeed * Time.deltaTime);
        }
        else
        {
            _stat.AddBaseValue(_supportDrainSpeed * Time.deltaTime * .1f);
        }
    }
}
