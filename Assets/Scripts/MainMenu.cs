using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static bool GameIsStart;
    public static bool FirstStart = true;

    public GameObject MobGenerator;

    public GameObject PlayerObj;

    private void Start()
    {
        GameIsStart = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerObj.SetActive(value: true);
            new Timer(.1f).SetEnd(() => GameIsStart = true);
            MobGenerator.SetActive(true);
            gameObject.SetActive(false);
            Score.CurrentScore = 0;
        }
    }
}
