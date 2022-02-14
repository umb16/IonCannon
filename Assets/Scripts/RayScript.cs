using System;
using UnityEngine;

public class RayScript : MonoBehaviour
{
	public GameObject MainObj;

	public GameObject SlowTile;

	public Light RayLight;

	public GameObject RayCylinder;

	private float _splash = 1f;

	public static Transform RayTransform;

	public TrailRenderer Trail;

	public AudioClip[] RaySounds;

	private bool stop;

	public void SetSplash(float splash)
	{
		_splash = splash;
	}

	private void Start()
	{
		GetComponent<AudioSource>().PlayOneShot(RaySounds[0]);
		MainObj.transform.localScale = Vector3.one * _splash;
		new Timers.Timer((Action)delegate
		{
			if (!stop)
			{
				GetComponent<AudioSource>().Play();
			}
		}, 0.2f, isFrameTimer: false);
		RayTransform = base.transform;
		RayCylinder.SetActive(value: true);
		Trail.startWidth = 0.3f * _splash;
		new Timers.Timer(delegate(Timers.Timer timer)
		{
			if (RayCylinder != null)
			{
				RayCylinder.transform.localScale = new Vector3(timer.NormalizedPlayTime * 0.5f * _splash, 40f, timer.NormalizedPlayTime * 0.5f * _splash);
			}
			RayLight.range = Mathf.SmoothStep(0f, 10f, timer.NormalizedPlayTime);
		}, 0f, 1f, 0f, delegate
		{
			MainObj.SetActive(value: true);
		});
	}

	public void Stop()
	{
		stop = true;
		new Timers.Timer((Action)delegate
		{
			GetComponent<AudioSource>().Stop();
		}, 0.01f, isFrameTimer: false);
		GetComponent<AudioSource>().PlayOneShot(RaySounds[1]);
		base.gameObject.GetComponentInChildren<ParticleSystem>().loop = false;
		new Timers.Timer(delegate(Timers.Timer timer)
		{
			RayCylinder.transform.localScale = new Vector3(0.5f * _splash - timer.NormalizedPlayTime * 0.5f * _splash, 40f, 0.5f * _splash - timer.NormalizedPlayTime * 0.5f * _splash);
			RayLight.range = Mathf.SmoothStep(10f, 0f, timer.NormalizedPlayTime);
			MainObj.transform.localScale = Vector3.one - Vector3.one * timer.NormalizedPlayTime;
		}, 0f, 0.5f, 0f, delegate
		{
			UnityEngine.Object.Destroy(RayCylinder);
		});
		new Timers.Timer(delegate
		{
			base.transform.position += Vector3.forward;
		}, 0f, 20f, 0f, delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	private void Update()
	{
	}
}
