using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : AiStateBase
{
    Vector3 _movePos;
    AIBase _ai;
    Rigidbody _rigidbody;
    float _maxDistance = 5f;
    float _minDistance = 2f;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _movePos = _ai.transform.position;
        _rigidbody = _ai.GetComponent<Rigidbody>();
    }

    public void MiniUpdate()
    {
        if(Random.Range(0,100) < 50)
        {
            RaycastHit hit;
            float right_dist = 9999f;
            float left_dist = 9999f;
            if (Physics.Raycast(_ai.transform.position, _ai.transform.right, out hit))
                right_dist = hit.distance;
            if (Physics.Raycast(_ai.transform.position, -_ai.transform.right, out hit))
                left_dist = hit.distance;

            if (left_dist > 2f && Random.Range(0, 100) < 50)
            {
                _movePos = CheckGround(_ai.transform.position + new Vector3(-Mathf.Min(Random.Range(_minDistance, _maxDistance), left_dist), 0f, 0f), left_dist);
            }
            else if(right_dist > 2f)
            {
                _movePos = CheckGround(_ai.transform.position + new Vector3(Mathf.Min(Random.Range(_minDistance, _maxDistance), right_dist), 0f, 0f), right_dist);
            }
        }
    }

    Vector3 CheckGround(Vector3 vector, float distance)
    {
        if (Physics.Raycast(vector, Vector3.down, 1.5f)) return vector; //have ground
        RaycastHit hit;
        if (Physics.Raycast(vector - new Vector3(0f, 1.5f, 0f), new Vector3(_ai.transform.position.x - vector.x, 0f, 0f), out hit, distance))
        {
            return new Vector3(hit.point.x, vector.y, vector.z);
        }
        else return _ai.transform.position; //cant see ground from AI to vector to move, return original position to stay
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
        vel.y = oldVel.y;
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x;
        vel.z = Mathf.Abs(oldVel.z) > Mathf.Abs(vel.z) ? oldVel.z : vel.z;
        _rigidbody.velocity = vel * Time.deltaTime;
    }
}
