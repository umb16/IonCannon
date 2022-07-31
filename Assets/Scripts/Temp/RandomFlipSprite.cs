using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlipSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.flipX = Random.value < .5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
