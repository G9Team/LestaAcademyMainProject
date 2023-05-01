using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace New
{

    public class BossSpikes : MonoBehaviour
    {
        [SerializeField] private float _spikesDelay;
        void Start()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(this.transform.DOMoveY(-1.8f, 0.01f));
            sequence.Append(this.transform.DOMoveY(0f, 0.5f));
            sequence.Append(this.transform.DOMoveY(0.01f, _spikesDelay));
            sequence.Append(this.transform.DOMoveY(25f, 2f));
            sequence.AppendCallback(DestroyCallback);
            sequence.Play();
        }
        void DestroyCallback()
        {
            Destroy(this.gameObject);
        }

    }
}
