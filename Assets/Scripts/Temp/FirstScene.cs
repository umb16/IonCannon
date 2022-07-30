using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FirstScene : MonoBehaviour
{
    
    void Start()
    {
        Addressables.LoadSceneAsync("Assets/Scenes/main.unity");
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
