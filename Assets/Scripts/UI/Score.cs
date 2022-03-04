using UnityEngine;
using Zenject;

public class Score : MonoBehaviour
{
    [SerializeField] ProgressBar _expProgressbar;
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }
    private void Update()
    {
        _expProgressbar.Set(_player.Exp.Normalized);
    }
}
