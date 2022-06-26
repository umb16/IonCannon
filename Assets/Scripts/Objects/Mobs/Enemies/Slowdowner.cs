using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using SPVD.LifeSupport;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Objects.Mobs.Enemies
{
    internal class Slowdowner : EnemySimple
    {
        [SerializeField] private Transform[] _palantedPoints;
        [SerializeField] private GameObject _projectileInstance;
        bool _attack = false;
        private LifeSupportTower _lifeSupportTower;
        //private float DistanceToPlayer => ((Player?.Position - Position) ?? Vector3.zero).magnitude;
        //private bool AttackIsAvaliable => _lifeSupportTower.InRadius(Position);
        [Inject]
        private void Construct(LifeSupportTower lifeSupportTower)
        {
            _lifeSupportTower = lifeSupportTower;
            //UniTaskAsyncEnumerable.EveryValueChanged(this, _ => AttackIsAvaliable).Subscribe(OnChangeState, this.GetCancellationTokenOnDestroy());
            //_moveTarget = _lifeSupportTower.GetRandomPoint();
            MoveTo(_lifeSupportTower.GetRandomPoint());
            SetBehaviour(BehaviorMethod);
        }

        private void OnShootFromAnim()
        {
            foreach (var point in _palantedPoints)
            {
                Instantiate(_projectileInstance, point.position+new Vector3(Random.value-.5f, Random.value - .5f,0), Quaternion.identity, transform.parent);
            }
        }

        private void StartAttack()
        {
            _attack = true;
            _animator.SetBool("planting", true);
            StopMove();
        }

        private void BehaviorMethod()
        {
            if (!_attack)
            {
                if (((Vector2)Position - _moveTarget).sqrMagnitude < 1)
                    StartAttack();
                //else
                //    MoveTo(_moveTarget);
                //if (Player != null)
                //    MoveTo(Player.Position+ new Vector3((1-Random.value*.5f)*2, (1 - Random.value * .5f)*2, 0));
            }
        }

    }
}
