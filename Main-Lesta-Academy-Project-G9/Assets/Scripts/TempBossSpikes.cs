using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossSpikes : MonoBehaviour
{

    private Vector3[] _path = new Vector3[]{
        new Vector3 (0, -1.8f, 0),
        new Vector3 (0, 0.38f, 0),
        new Vector3 (0, 0.4f, 0),
        new Vector3 (0, 10f, 0),
        };
    void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(this.transform.DOMoveY(-1.8f, 0.01f));
        sequence.Append(this.transform.DOMoveY(0f, 1f));
        sequence.Append(this.transform.DOMoveY(0.01f, 0.7f));
        sequence.Append(this.transform.DOMoveY(25f, 2f));
        sequence.AppendCallback(DestroyCallback);
        sequence.Play();
    }
    void DestroyCallback(){
        Destroy(this.gameObject);
    }

   
}
