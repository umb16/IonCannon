using UnityEngine;

public class Score : MonoBehaviour
{
	private static int _bestScore;

	private static int _currentScore;

	public static Score Self;

	public TextMesh ScoreText;

	public static int CurrentScore
	{
		get
		{
			return _currentScore;
		}
		set
		{
			_currentScore = value;
			Refesh();
		}
	}

	private static void Refesh()
	{
		Self.ScoreText.text = "BEST SCORE: " + _bestScore + "\nSCORE: " + _currentScore;
	}

	private void Start()
	{
		Self = this;
		_bestScore = PlayerPrefs.GetInt("BScore", 0);
		if (CurrentScore > _bestScore)
		{
			_bestScore = _currentScore;
			PlayerPrefs.SetInt("BScore", _bestScore);
		}
		_currentScore = 0;
		ScoreText.text = string.Empty;
	}

	private void Update()
	{
	}
}
