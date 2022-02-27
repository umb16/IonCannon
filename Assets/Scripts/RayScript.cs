using System;
using UnityEngine;

public class RayScript : WithTimer
{
    public GameObject MainObj;

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
        CreateTimer(.2f)
            .SetEnd(() =>
            {
                if (!stop)
                {
                    GetComponent<AudioSource>().Play();
                }
            });
        RayCylinder.SetActive(value: true);
        Trail.startWidth = 0.3f * _splash;
        CreateTimer(1)
            .SetUpdate((x) =>
            {
                if (RayCylinder != null)
                {
                    RayCylinder.transform.localScale = new Vector3(x * 0.5f * _splash, x * 0.5f * _splash * .7f, 1);
                }
            })
            .SetEnd(() => MainObj.SetActive(true));
    }

    public void Stop()
    {
        stop = true;
        CreateTimer(.01f)
            .SetEnd(() => GetComponent<AudioSource>().Stop());
        GetComponent<AudioSource>().PlayOneShot(RaySounds[1]);
        gameObject.GetComponentInChildren<ParticleSystem>().loop = false;
        CreateTimer(.5f)
            .SetUpdate((x) =>
            {
                RayCylinder.transform.localScale = new Vector3(0.5f * _splash - x * 0.5f * _splash, (0.5f * _splash - x * 0.5f * _splash) * .7f, 1);
                MainObj.transform.localScale = Vector3.one - Vector3.one * x;
            })
            .SetEnd(() =>
            {
                MainObj.SetActive(false);
                Destroy(RayCylinder);
            });
        CreateTimer(20)
            .SetUpdate((x) => transform.position += Vector3.forward)
            .SetEnd(() => Destroy(gameObject));
    }
}
