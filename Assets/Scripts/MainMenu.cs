using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static bool gameIsStart;

    public GameObject MobGenerator;

    public GameObject PlayerObj;

    private void Start()
    {
        gameIsStart = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerObj.SetActive(value: true);
            new Timer(1).SetEnd(() => gameIsStart = true);
            MobGenerator.SetActive(value: true);
            base.gameObject.SetActive(value: false);
            Score.CurrentScore = 0;
        }
    }
}
