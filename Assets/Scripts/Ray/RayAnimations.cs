using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAnimations : MonoBehaviour, IRayAnimation
{
    [SerializeField] RaySpotAnimation[] _animations;
    public void Set(float size)
    {
        foreach (var anim in _animations)
        {
            anim.Set(size);
        }
    }
}
