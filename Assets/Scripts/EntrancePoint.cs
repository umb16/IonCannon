using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrancePoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BaseLayer.Show<MainMenu>();
        BaseLayer.Show<EndScreen>().Hide();

    }

}
