using Cysharp.Threading.Tasks;
using System.Collections;
using Umb16.Extensions;
using UnityEngine;
using Zenject;

public class ArtilleryProjectile : MonoBehaviour
{
    [SerializeField] private GameObject _startEffect;
    [SerializeField] private GameObject _endEffect;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private StandartZoneIndicator _zone;
    [SerializeField] private float _splashRadius;
    [SerializeField] private float _damage;
    [SerializeField] private float _fallDelay;
    private Timer _timer;
    private Player _player;
    [Inject]
    private void Construct(AsyncReactiveProperty<Player> player)
    {
        _player = player;
    }

    private void Start()
    {
        _startEffect.SetActive(true);

        _zone.gameObject.SetActive(true);
        Vector3 landingPosition = _player.Position.GetRandomPointInRadius(_splashRadius * 2);
        _zone.SetPosition(landingPosition);
        _zone.SetRadius(0);
        //_zone.SetBlink(.2f);
        _endEffect.transform.position = landingPosition.Get2D() + Vector3.back * 100;
        _timer = new Timer(_fallDelay)
            .SetUpdate(x =>_zone.SetRadius(_splashRadius * x * .5f))
            .SetEnd(() =>
            {
                _endEffect.SetActive(true);

                _timer = new Timer(1)
                .SetUpdate(x => _zone.SetRadius(_splashRadius * (x * .5f + .5f)))
                .SetEnd(() =>
                {
                    Instantiate(_explosion, landingPosition.Get2D(), Quaternion.identity, transform.parent);
                    if ((_player.Position - landingPosition).MagnetudeXY() < _splashRadius)
                    {
                        _player.ReceiveDamage(new DamageMessage(null, _player, _damage, DamageSources.Explosion, .1f));
                    }
                    Destroy(gameObject);
                });
            });
    }

    private void OnDestroy()
    {
        _timer?.Stop();
    }
}
