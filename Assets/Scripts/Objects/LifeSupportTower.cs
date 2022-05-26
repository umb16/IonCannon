using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
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
    private AsyncReactiveProperty<Player> _player;
    ComplexStat _stat;


    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;
        _player.Where(x => x != null).ForEachAsync(x =>
        {
            _stat = x.StatsCollection.GetStat(StatType.LifeSupport);
        });

        _lineRenderer.positionCount = _vertexCount;
        for (int i = 0; i < _vertexCount; i++)
        {
            Vector3 newPoint = Vector3.up.DiamondRotateXY(4.0f / _vertexCount * i);
            //newPoint.y *= .8f;
            newPoint *= _radius;

            _lineRenderer.SetPosition(i, newPoint.Get2D());
        }
    }
    void Update()
    {
        if (_player.Value == null)
            return;
        if (_player.Value.Position.SqrMagnetudeXY() > _radius * _radius)
        {
            _stat.AddBaseValue(-_supportDrainSpeed * Time.deltaTime);
        }
        else
        {
            _stat.AddBaseValue(_supportDrainSpeed * Time.deltaTime * .1f);
        }
    }
}
