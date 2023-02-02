using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformMover : MonoBehaviour
{
    
    void Start()
    {
        this.transform.DOMoveY(this.transform.position.y - 2f, 0.2f, false).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

}
