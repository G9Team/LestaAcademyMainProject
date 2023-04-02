using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFly : AiStateBase
{
    Vector3 _movePos;
    AIBase _ai;
    Rigidbody _rigidbody;
    float _maxDistance = 10f;
    float _minDistance = 4f;
    public float leftLimit = 8f;
    public float rightLimit = 8f;
    float _initX = 0f;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _movePos = _ai.transform.position;
        _rigidbody = _ai.GetComponent<Rigidbody>();
        _initX = _ai.transform.position.x;
    }

    public void MiniUpdate()
    {
        if(Random.Range(0,100) < 50)
        {
            RaycastHit hit;
            float right_dist = 9999f;
            float left_dist = 9999f;
            if (Physics.Raycast(_ai.transform.position, Vector3.right, out hit))
                right_dist = hit.distance;
            if (Physics.Raycast(_ai.transform.position, Vector3.left, out hit))
                left_dist = hit.distance;

            if (left_dist > 2f && Random.Range(0, 100) < 50)
            {
                _movePos = new Vector3(Mathf.Max(_ai.transform.position.x - Mathf.Min(Random.Range(_minDistance, _maxDistance), left_dist), _initX-leftLimit), _ai.transform.position.y, _ai.transform.position.z);
            }
            else if(right_dist > 2f)
            {
                _movePos = new Vector3(Mathf.Min(_ai.transform.position.x + Mathf.Min(Random.Range(_minDistance, _maxDistance), right_dist), _initX + leftLimit), _ai.transform.position.y, _ai.transform.position.z);
            }
        }
    }

    public void Update()
    {
        _movePosition(_movePos);
    }

    void _movePosition(Vector3 position)
    {
        Vector3 oldVel = _rigidbody.velocity;
        Vector3 delta = position - _rigidbody.position;
        Vector3 vel = delta / Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(_ai.transform.position, Vector3.down, out hit, 100f, ~LayerMask.GetMask("Player")))
        {
            if (hit.distance < 4f)
            {
                vel.y = (4f-hit.distance)*1000;
            }
            else
            {
                vel.y = Physics.gravity.y*25;
            }
        }
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x;
        vel.z = Mathf.Abs(oldVel.z) > Mathf.Abs(vel.z) ? oldVel.z : vel.z;
        _rigidbody.velocity = vel * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(_ai.transform.position - new Vector3(position.x, _ai.transform.position.y, position.z));
        _ai.transform.rotation = Quaternion.Slerp(_ai.transform.rotation, targetRotation, 5f * Time.deltaTime);
    }
}
