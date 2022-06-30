using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private List<Timer> _timers = new List<Timer>();

    public void SetText(string text)
    {
        _text.text = text;
    }
    void OnEnable()
    {
        _text.color = Color.white;
        _timers.Add(new Timer(.3f)
            .SetUpdate(x =>
            {
                float tension = 5;
                x -= 1;
                x = x * x * ((tension + 1) * x + tension) + 1;
                transform.position += Vector3.back * Time.deltaTime;
                transform.localScale = Vector3.LerpUnclamped(Vector3.one * .1f, Vector3.one, x);
            })
            .SetEnd(() =>
            {
                Color startColor = _text.color;
                Color endColor = startColor;
                endColor.a = 0;
                _timers.Add(new Timer(.9f)
                    .SetUpdate(x =>
                    {
                        transform.position += Vector3.back * Time.deltaTime * (1 -x*2f);
                        _text.color = Color.Lerp(startColor, endColor, Mathf.Max(0, x - .5f));
                    })
                    .SetEnd(() => Destroy(gameObject)));
            }));
    }
    private void OnDestroy()
    {
        foreach (var timer in _timers)
        {
            timer.Stop();
        }
    }
}
