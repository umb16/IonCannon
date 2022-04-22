using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryTrashCan : MonoBehaviour
{
    [SerializeField] private AudioSource _audoioSource;
    public void PlaySound()
    {
        _audoioSource.Play();
    }
}
