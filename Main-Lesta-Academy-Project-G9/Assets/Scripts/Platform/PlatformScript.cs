using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformScript : MonoBehaviour
{
    
    private enum TYPE_PLATFORM
    {
        GHOST,
        ROTATE,
        MOVE,
        MOVE_WITH_ROTATE

    }

   
    [SerializeField] private TYPE_PLATFORM platformType;
    [SerializeField] private float _time;
    [SerializeField] private int _loop;
    [SerializeField] private Vector3[] _waypoints;

    void Start()
    {
        switch (platformType)
        {
            case TYPE_PLATFORM.GHOST:
                this.GetComponentInChildren<GhostPlatform>().enabled = true;
            break;

            case TYPE_PLATFORM.ROTATE:
                RotateAround();
            break;

            case TYPE_PLATFORM.MOVE:
                MoveToPoints();
            break;

            case TYPE_PLATFORM.MOVE_WITH_ROTATE:
                MoveWithRotate();
            break;
     
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
        
            other.transform.parent = transform;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }


    private void RotateAround()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 360), _time, RotateMode.FastBeyond360).SetLoops(_loop).SetEase(Ease.Linear);
    }

    private void MoveToPoints()
    {
        transform.DOPath(_waypoints, _time, PathType.Linear).SetLoops(_loop, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void MoveWithRotate()
    {
        DOTween.Sequence()
            .Append(transform.DOPath(_waypoints, _time, PathType.Linear).SetLoops(_loop, LoopType.Yoyo).SetEase(Ease.Linear))
            .Join(transform.DOLocalRotate(new Vector3(0, 0, 360), _time, RotateMode.FastBeyond360).SetLoops(_loop).SetEase(Ease.Linear));       
    }
}
