using UnityEngine;

public class MobGenerator : MonoBehaviour
{
	public static MobGenerator Self;

	public GameObject[] MobPrafab;

	private int _currenWave;

	private float[][] _waves = new float[10][]
	{
		new float[9]
		{
			0.84f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.001f,
			0.05f
		},
		new float[9]
		{
			0.66f,
			33f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0.001f,
			0f
		},
		new float[9]
		{
			0.49f,
			0.4f,
			0.1f,
			0f,
			0f,
			0f,
			0f,
			0.001f,
			0f
		},
		new float[9]
		{
			0.39f,
			0.4f,
			0.1f,
			0.1f,
			0f,
			0f,
			0f,
			0.001f,
			0f
		},
		new float[9]
		{
			0.29f,
			0.3f,
			0.1f,
			0.1f,
			0.2f,
			0f,
			0f,
			0.001f,
			0f
		},
		new float[9]
		{
			0.19f,
			0.2f,
			0.2f,
			0.2f,
			0.1f,
			0.01f,
			0f,
			0.001f,
			0f
		},
		new float[9]
		{
			0.19f,
			0.2f,
			0.1f,
			0.2f,
			0.1f,
			0.02f,
			0.1f,
			0.001f,
			0f
		},
		new float[9]
		{
			0.2f,
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.2f,
			0.03f,
			0.1f,
			0f
		},
		new float[9]
		{
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			1f,
			0f
		},
		new float[9]
		{
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.01f,
			0.2f
		}
	};

	private int[] _wawesMobCount = new int[10]
	{
		20,
		20,
		20,
		25,
		35,
		40,
		45,
		50,
		55,
		60
	};

	private int currentLoop;

	private int _createMobsLeft = 20;

	private int _waveMobCounter;

	private float _time;

	private float _bossTime;

	public int WaveMobCounter
	{
		get
		{
			return _waveMobCounter;
		}
		set
		{
			_waveMobCounter = value;
			if (_waveMobCounter >= _wawesMobCount[_currenWave] * (currentLoop + 1))
			{
				NextWave();
			}
		}
	}

	private void NextWave()
	{
		_currenWave++;
		if (_currenWave >= _waves.Length)
		{
			_currenWave = 0;
			currentLoop++;
		}
		_waveMobCounter = 0;
		_createMobsLeft = _wawesMobCount[_currenWave] * (currentLoop + 1);
	}

	private void CrateMob()
	{
		if (_createMobsLeft > 0)
		{
			_createMobsLeft--;
			Vector2 vector = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f);
			vector.Normalize();
			vector *= 25f;
			GameObject gameObject = Object.Instantiate(MobPrafab[GetRandomMob()], new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity) as GameObject;
			MobOld component = gameObject.GetComponent<MobOld>();
			component.Hp *= currentLoop + 1;
			component.ScoreCost *= currentLoop + 1;
			if (Random.value < 0.05f)
			{
				gameObject.transform.localScale *= 1.5f;
				component.Hp *= 1.5f;
				component.ScoreCost = (int)((float)component.ScoreCost * 1.5f);
			}
		}
		Invoke("CrateMob", (Random.value + 2f) / (Mathf.Abs(Mathf.Sin(((float)Score.CurrentScore + _time) / 100f)) + 1f));
	}

	private void CreateBoss()
	{
		GetComponent<AudioSource>().Play();
		Vector2 vector = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f);
		vector.Normalize();
		vector *= 25f;
		GameObject gameObject = Object.Instantiate(MobPrafab[Random.Range(0, MobPrafab.Length)], new Vector3(vector.x, vector.y, -0.5f), Quaternion.identity) as GameObject;
		MobOld component = gameObject.GetComponent<MobOld>();
		component.ScoreCost *= currentLoop + 1;
		component.Hp += currentLoop * 10;
		component.Hp *= 17f;
		gameObject.transform.localScale *= 2f;
		component.ScoreCost *= 6;
	}

	private int GetRandomMob()
	{
		float value = Random.value;
		float num = 0f;
		for (int i = 0; i < _waves[_currenWave].Length; i++)
		{
			if (value > num && value < num + _waves[_currenWave][i])
			{
				return i;
			}
			num += _waves[_currenWave][i];
		}
		return 0;
	}

	private void Start()
	{
		Self = this;
		_createMobsLeft = _wawesMobCount[0];
		Invoke("CrateMob", 1f);
	}

	private void Update()
	{
		_bossTime += Time.deltaTime;
		if (_bossTime > 100f)
		{
			CreateBoss();
			_bossTime = 0f;
		}
		_time += Time.deltaTime;
	}
}
