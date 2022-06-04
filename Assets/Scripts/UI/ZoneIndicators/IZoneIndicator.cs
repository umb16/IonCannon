using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZoneIndicator
{
    void SetPosition(Vector2 position);
    void SetRadius(float radius);
    void SetIcon(AddressKeys addressKey);
    void Hide();
}
