using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreListenerPause : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (_audioSource != null)
            _audioSource.ignoreListenerPause = true;
    }
}
