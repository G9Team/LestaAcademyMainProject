using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPosition : MonoBehaviour
{
    [SerializeField] public List<Transform> UpperPositions, MidPostitons, LowSidePositions;
    [SerializeField] private GameObject _bossBody, _particles;
    private List<Transform> _positions = new List<Transform>();
    public Transform CurrentPosition { get; private set; }

    private void Awake()
    {
        _positions.AddRange(LowSidePositions);
        _positions.AddRange(MidPostitons);
        _positions.AddRange(UpperPositions);
    }

    public void ChangePosition()
    {
        Transform tempPosition;
        do
        {
            tempPosition = _positions[Random.Range(0, _positions.Count)];
        } while (tempPosition == CurrentPosition);
        ChangePosition(tempPosition);
    }
    
    public void ChangePosition(Transform position)    
    {
        _particles.SetActive(true);

        if (CurrentPosition == position) return;
        //TODO made some async (coroutine) for customisation, track transition
        CurrentPosition = position;
        _bossBody.transform.position = position.position;
        _bossBody.transform.rotation = position.rotation;
        _particles.SetActive(true);


    }
    public void ChangePositionToSafe(){
        ChangePosition(UpperPositions[0]);
    }

}
