using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryTrashCan : MonoBehaviour
{
    public void PlaySound()
    {
        SoundManager.Instance.PlayDisassemble();
    }
}
