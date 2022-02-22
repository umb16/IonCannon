using System;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	public GameObject Barr;

	public GameObject Expl;

	public GameObject Particle;

	private void Start()
	{
	}

	private void OnTriggerEnter(Collider collision)
	{
		if ((collision.gameObject.tag == "Ray" || collision.gameObject.tag == "Barrel") && Barr.activeSelf)
		{
			Barr.SetActive(value: false);
			Expl.SetActive(value: true);
			new Timer(()=>
			{
				if (Expl != null)
				{
					Expl.SetActive(value: false);
				}
			}, 0.1f);
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(Particle, base.transform.position + Vector3.back * 0.5f, Particle.transform.rotation), 10f);
			GetComponent<Collider>().enabled = false;
		}
	}

	private void Update()
	{
	}
}
