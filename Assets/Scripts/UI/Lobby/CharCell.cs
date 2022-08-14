using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCell : MonoBehaviour
{
    [SerializeField] private Image[] _images;

    public void SetSelected(bool value)
    {
        foreach (var image in _images)
        {
            image.color = value ? Color.white : Color.gray;
        }
    }
}
