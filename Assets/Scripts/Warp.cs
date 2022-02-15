using UnityEngine;

public class Warp : MonoBehaviour
{
    public GameObject Barr;

    public GameObject Beam;

    private void Start()
    {
        Beam.SetActive(value: true);
        new Timers.Timer((x) =>
        {
            Beam.transform.localScale = new Vector3(x * 1f, 40f, x * 1f);
        }, delegate
        {
            Barr.SetActive(value: true);
            GetComponent<Collider>().enabled = true;
            new Timers.Timer((x) =>
            {
                if (Beam != null)
                {
                    Beam.transform.localScale = new Vector3(1f - x * 1f, 40f, 1f - x * 1f);
                }
            }, () =>
            {
                if (Beam != null)
                {
                    Beam.SetActive(value: false);
                }
            }, 0.5f, 0f);
        }, 0.5f, 0f);
    }

    private void Update()
    {
    }
}
