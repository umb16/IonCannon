using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAnim : MonoBehaviour, IRayAnimation
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public void Set(float size)
    {
        var spriteSize = _spriteRenderer.size;
        if (size < 1)
            size *= 8;
        else
            size = 7 + size;
        spriteSize.x =  size;
        _spriteRenderer.size = spriteSize;
    }
}
