using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
    private enum TYPE_PLATFORM
    {
        ROTATE,
        MOVE,
        MOVE_WITH_ROTATE

    }

    [Header("Ghost Settings")]
    private Animator _ghostAnim;



    [SerializeField] float disappearTime = 3;
    [SerializeField] bool canReset;
    [SerializeField] float resetTime;

    [SerializeField] private TYPE_PLATFORM platformType;
    [SerializeField] private float _time;
    private int _loop;



    [SerializeField] private Vector3[] _waypoints;
    [SerializeField] private bool isGhost;
    public bool isLooped;




    void Start()
    {
        if (isLooped) _loop = -1;
        else _loop = 0;

        _ghostAnim = GetComponentInChildren<Animator>();
        _ghostAnim.SetFloat("DissapearTime", 1 / disappearTime);

        switch (platformType)
        {
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

    public void SayToMove()
    {
        isLooped = true;

        switch (platformType)
        {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (platformType == TYPE_PLATFORM.MOVE) collision.transform.parent = transform;
        if (isGhost) _ghostAnim.SetBool("Trigger", true);
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
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

    public void TriggerReset()
    {
        if (canReset)
        {
            StartCoroutine(Reset());
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetTime);

        _ghostAnim.SetBool("Trigger", false);

    }
}
