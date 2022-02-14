using UnityEngine;

public class Warp : MonoBehaviour
{
	public GameObject Barr;

	public GameObject Beam;

	private void Start()
	{
		Beam.SetActive(value: true);
		new Timers.Timer(delegate(Timers.Timer timer)
		{
			Beam.transform.localScale = new Vector3(timer.NormalizedPlayTime * 1f, 40f, timer.NormalizedPlayTime * 1f);
		}, 0f, 0.5f, 0f, delegate
		{
			Barr.SetActive(value: true);
			GetComponent<Collider>().enabled = true;
			new Timers.Timer(delegate(Timers.Timer timer)
			{
				if (Beam != null)
				{
					Beam.transform.localScale = new Vector3(1f - timer.NormalizedPlayTime * 1f, 40f, 1f - timer.NormalizedPlayTime * 1f);
				}
			}, 0f, 0.5f, 0f, delegate
			{
				if (Beam != null)
				{
					Beam.SetActive(value: false);
				}
			});
		});
	}

	private void Update()
	{
	}
}
