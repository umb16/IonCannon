using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
   // [EditorButton]
    public void Set(float value)
    {
        transform.localScale = new Vector3(value, 1, 1);
    }
}
