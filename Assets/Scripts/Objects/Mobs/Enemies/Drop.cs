using System;
using UnityEngine;

[Serializable]
public struct Drop
{
    public float Probability;
    public GameObject GameObject;
    public void Release(Vector3 position, float factor = 1)
    {
        float probability = Probability * factor;
        while (probability >= 1)
        {
            probability--;
            GameObject.Instantiate(GameObject, position, Quaternion.identity);
        }
        if (UnityEngine.Random.value < probability)
        {
            GameObject.Instantiate(GameObject, position, Quaternion.identity);
        }
    }
}
