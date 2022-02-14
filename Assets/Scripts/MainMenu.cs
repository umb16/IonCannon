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
			new Timers.Timer((Action)delegate
			{
				gameIsStart = true;
			}, 1f, isFrameTimer: false);
			MobGenerator.SetActive(value: true);
			base.gameObject.SetActive(value: false);
			Score.CurrentScore = 0;
		}
	}
}
