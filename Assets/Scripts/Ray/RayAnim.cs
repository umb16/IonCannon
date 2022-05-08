using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAnim : MonoBehaviour, IRayAnimation
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public void Set(float size)
    {
        Debug.Log(size);
        var spriteSize = _spriteRenderer.size;
        spriteSize.x =  size * 8;
        _spriteRenderer.size = spriteSize;
    }
}
