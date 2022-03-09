using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umb16.Extensions;
using Zenject;

public class GrassSetter : MonoBehaviour
{
    [SerializeField] GameObject[] _grass;
    private List<Transform> _grasses = new List<Transform>();
    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Start()
    {
        float shift = Random.value * 100;
        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 50; j++)
            {
                if (i * i + j * j > 2500)
                    continue;
                float x = i + 50 + shift;
                float y = j + 50 + shift;
                float perlin = (Mathf.PerlinNoise(x * 1.1f, y * 1.1f) * Mathf.PerlinNoise(y * 3.1f, x * 3.1f));
                if (perlin > .6f)
                {
                    var go = Instantiate(_grass[Random.Range(0, _grass.Length)]);
                    go.transform.SetParent(transform);
                    go.transform.Set2DPos(i * .5f, j * .5f);
                    if (Random.value > .5f)
                        go.transform.localScale = new Vector3(1, 1, 1);
                    else
                        go.transform.localScale = new Vector3(-1, 1, 1);
                    _grasses.Add(go.transform);
                }
            }
        }

    }

    private void Update()
    {
      /* foreach (var grass in _grasses)
        {
            Vector3 dir = _player.transform.position - grass.position;
            if ((dir).SqrMagnetudeXZ() > 60 * 60)
            {
                grass.position = grass.position + dir * .9f; 
            }
        }*/
    }
}
