using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera : MonoBehaviour
{

    void Start()
    {
        UniTaskAsyncEnumerable.EveryValueChanged(Camera.main.transform, x => x.eulerAngles.x).Subscribe(x =>
        {
            var vector = transform.eulerAngles;
            vector.x = x;
            transform.eulerAngles = vector;
        },cancellationToken: this.GetCancellationTokenOnDestroy());
    }


}
