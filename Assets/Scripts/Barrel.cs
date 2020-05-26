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
        if ((collision.gameObject.CompareTag("Ray") || collision.gameObject.CompareTag("Barrel")) && Barr.activeSelf)
        {
            Barr.SetActive(value: false);
            Expl.SetActive(value: true);
            new Timers.Timer((Action)delegate
            {
                if (Expl != null)
                {
                    Expl.SetActive(value: false);
                }
            }, 0.1f, isFrameTimer: false);
            Destroy(Instantiate(Particle, transform.position + Vector3.back * 0.5f, Particle.transform.rotation), 10f);
            GetComponent<Collider>().enabled = false;
        }
    }
}