using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private AudioSource _sound;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private ItemId[] _itemTypes;
    private Player _player;
    private Item _item;
    private bool _taken;
    private Timer _timer;
    [Inject]
    private async void Construct(AsyncReactiveProperty<Player> player, ItemsDB itemsDB)
    {
        _player = player;
        var itemType = _itemTypes[Random.Range(0, _itemTypes.Length)];
        
         _item   = itemsDB.CreateItem(itemType);
        _renderer.sprite = await Addressables.LoadAssetAsync<Sprite>(_item.IconAddress).Task;
        _renderer.size = new Vector2(1, 1); 
        Debug.Log("_renderer.sprite"+ _renderer.size);

    }

    private void Update()
    {
        if (_taken)
            return;
        if (_player != null && (_player.Position - transform.position).magnitude < _player.StatsCollection.GetStat(StatType.PickupRadius).Value)
        {
            _taken = true;
            _sound.Play();
            _player.Inventory.Add(_item);
            Vector3 startPosition = transform.position;
            _timer = new Timer(.5f)
                .SetUpdate(x => transform.position = Vector3.Lerp(startPosition, _player.Position, x))
                .SetEnd(()=>Destroy(gameObject));
        }
    }

    private void OnDestroy()
    {
        _timer?.Stop();
    }
}
