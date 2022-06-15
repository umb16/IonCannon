using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreListenerPause : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().ignoreListenerPause = true;
    }
}
