using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Umb16.Extensions;

public class ArrowToShip : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    private ShopShip _ship;
    private AsyncReactiveProperty<Player> _player;

    [Inject]
    private void Construct(ShopShip ship, AsyncReactiveProperty<Player> player)
    {
        _ship = ship;
        _player = player;
        UniTaskAsyncEnumerable.EveryValueChanged(_ship, x => x.Landed).Subscribe(x => _arrow.SetActive(x));
    }

    private void Update()
    {
        if (_ship.Landed)
        {
            transform.position = _player.Value.Position + Vector3.up*1;
            transform.transform.eulerAngles = new Vector3(0,0,(_player.Value.Position - _ship.transform.position).DiamondAngleXY4()*90);
        }
    }
}
