using UnityEngine;

public class Warp : MonoBehaviour
{
    public GameObject Barr;

    public GameObject Beam;

    private void Start()
    {
        Beam.SetActive(value: true);
        new Timer(.5f)
            .SetUpdate((x) =>Beam.transform.localScale = new Vector3(x * 1f, 40f, x * 1f))
            .SetEnd(()=>
            {
                Barr.SetActive(value: true);
                GetComponent<Collider>().enabled = true;
                new Timer(.5f).SetUpdate((x) =>
                {
                    if (Beam != null)
                    {
                        Beam.transform.localScale = new Vector3(1f - x * 1f, 40f, 1f - x * 1f);
                    }
                })
                .SetEnd(() =>
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
