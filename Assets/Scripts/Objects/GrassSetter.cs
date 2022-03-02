using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSetter : MonoBehaviour
{
    [SerializeField] GameObject[] _grass;
    private void Start()
    {

        /* for (int i = 0; i < 1000; i++)
         {
             var go = Instantiate(_grass[Random.Range(0, _grass.Length)]);
             int y = Random.Range(-20, 20);
             go.transform.position = new Vector3(Random.Range(-20, 20), y, y * .1f);
         }*/
        float shift = Random.value * 100;
        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 50; j++)
            {
                float x = i + 50 + shift;
                float y = j + 50 + shift;
                float perlin = (Mathf.PerlinNoise(x * 1.1f, y * 1.1f) * Mathf.PerlinNoise(y * 3.1f, x * 3.1f));
                if (perlin > .6f)
                {
                    var go = Instantiate(_grass[Random.Range(0, _grass.Length)]);
                    go.transform.position = new Vector3(i * .5f, j * .5f, j * .5f * .1f);
                    if (Random.value > .5f)
                        go.transform.localScale = new Vector3(1, 1, 1);
                    else
                        go.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

    }
}
