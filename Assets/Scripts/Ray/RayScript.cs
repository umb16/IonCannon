using System;
using UnityEngine;
using Zenject;

public class RayScript : WithTimer
{
    public GameObject MainObj;

   [SerializeField] private RayAnimations _rayAnim;
   [SerializeField] private ParticleSystem _particleSystem;

    private float _splash = 1f;

    public static Transform RayTransform;

    //public TrailRenderer Trail;

    public AudioClip[] RaySounds;

    private bool stop;

    private Timer _timer;

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
        _rayAnim.gameObject.SetActive(true);
        _timer = CreateTimer(.2f)
            .SetUpdate((x) =>
            {
                if (_rayAnim != null)
                {
                    _rayAnim.Set(x * _splash);
                }
            })
            .SetEnd(() => MainObj.SetActive(true));
    }

    public void Stop()
    {
        _timer?.Stop();
        stop = true;
        CreateTimer(.01f)
            .SetEnd(() => GetComponent<AudioSource>().Stop());
        GetComponent<AudioSource>().PlayOneShot(RaySounds[1]);
        if (_particleSystem != null)
        {
            var main = _particleSystem.main;
            main.loop = false;
        }
        CreateTimer(.2f)
            .SetUpdate((x) =>
            {
                _rayAnim.Set(_splash - x * _splash);
                MainObj.transform.localScale = Vector3.one - Vector3.one * x;
            })
            .SetEnd(() =>
            {
                MainObj.SetActive(false);
                _rayAnim.gameObject.SetActive(false);
            });
        CreateTimer(10)
            .SetEnd(() => Destroy(gameObject));
    }
}
