using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private bool InvisibleOnFull;
    // [EditorButton]
    public void Set(float value)
    {
        if (InvisibleOnFull && value == 1)
            transform.localScale = new Vector3(0, 1, 1);
        else
            transform.localScale = new Vector3(value, 1, 1);
    }
}
