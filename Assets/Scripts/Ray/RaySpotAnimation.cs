using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySpotAnimation : MonoBehaviour, IRayAnimation
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;
    private int _max = 8;

    [EditorButton]
    public void Set(float size)
    {
        if (size == 0)
            _spriteRenderer.enabled = false;
        else
            _spriteRenderer.enabled = true;

        float specialSize = size * 16;
        specialSize = Mathf.Min(specialSize, _max);
        int index = (int)(specialSize / _max * (_sprites.Length - 1));
        _spriteRenderer.sprite = _sprites[index];
    }
}
