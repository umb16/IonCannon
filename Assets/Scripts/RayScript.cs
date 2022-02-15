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
        }, 0.2f);
        RayTransform = base.transform;
        RayCylinder.SetActive(value: true);
        Trail.startWidth = 0.3f * _splash;
        new Timers.Timer((x) =>
        {
            if (RayCylinder != null)
            {
                RayCylinder.transform.localScale = new Vector3(x * 0.5f * _splash, 40f, x * 0.5f * _splash);
            }
            RayLight.range = Mathf.SmoothStep(0f, 10f, x);
        }, () =>
       {
           MainObj.SetActive(value: true);
       }, 1f);
    }

    public void Stop()
    {
        stop = true;
        new Timers.Timer(() =>
        {
            GetComponent<AudioSource>().Stop();
        }, 0.01f);
        GetComponent<AudioSource>().PlayOneShot(RaySounds[1]);
        gameObject.GetComponentInChildren<ParticleSystem>().loop = false;
        new Timers.Timer((x) =>
        {
            RayCylinder.transform.localScale = new Vector3(0.5f * _splash - x * 0.5f * _splash, 40f, 0.5f * _splash - x * 0.5f * _splash);
            RayLight.range = Mathf.SmoothStep(10f, 0f, x);
            MainObj.transform.localScale = Vector3.one - Vector3.one * x;
        }, () =>
        {
            Destroy(RayCylinder);
        }, 0.5f, 0f);
        new Timers.Timer((x) =>
        {
            transform.position += Vector3.forward;
        }, () =>
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }, 20f, 0f);
    }
}
