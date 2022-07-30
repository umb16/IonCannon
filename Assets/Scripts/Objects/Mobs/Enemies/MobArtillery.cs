using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using SPVD.LifeSupport;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Objects.Mobs.Enemies
{
    internal class MobArtillery : EnemySimple
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private GameObject _projectileInstance;
        bool _attack = false;
        bool _canMove = true;
        private LifeSupportTower _lifeSupportTower;
        private float DistanceToPlayer => ((Player?.Position - Position) ?? Vector3.positiveInfinity).magnitude;
        private bool AttackIsAvaliable => (DistanceToPlayer < (_attack ? 30 : 20)) && (_lifeSupportTower.InRadius(Position) || DistanceToPlayer < 10);

        [Inject]
        private void Construct(LifeSupportTower lifeSupportTower)
        {
            _lifeSupportTower = lifeSupportTower;
            UniTaskAsyncEnumerable.EveryValueChanged(this, _ => AttackIsAvaliable).Subscribe(OnChangeState, this.GetCancellationTokenOnDestroy());
            SetBehaviour(BehaviorMethod);
        }

        private void OnShootFromAnim()
        {
            Instantiate(_projectileInstance, _firePoint.position, Quaternion.identity,  transform.parent);

            if (!_attack)
            {
                _canMove = true;
            }
        }

        private void OnChangeState(bool attack)
        {
            _attack = attack;
            Animator.SetBool("attack", attack);
            if (attack)
            {
                _canMove = false;
                StopMove();
            }
        }

        private void BehaviorMethod()
        {
            if (!_attack && _canMove)
            {
                if (Player != null)
                    MoveTo(Player.Position+ new Vector3((1-Random.value*.5f)*2, (1 - Random.value * .5f)*2, 0));
            }
        }

    }
}
