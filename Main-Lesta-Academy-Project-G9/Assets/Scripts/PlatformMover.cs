using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformMover : MonoBehaviour
{
    
    void Start()
    {
        this.transform.DOMoveY(41f, 4f, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuart);
    }

}
