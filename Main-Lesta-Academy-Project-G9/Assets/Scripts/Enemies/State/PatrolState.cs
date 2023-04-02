using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : AiStateBase
{
    AIBase _ai;
    Rigidbody _rigidbody;
    float _moveDir = 0;
    public float speed = 200f;
    BoxCollider _collider;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _rigidbody = _ai.GetComponent<Rigidbody>();
        _collider = _ai.GetComponent<BoxCollider>();
        _moveDir = 1f;
    }

    public void MiniUpdate()
    {
    }

    void CheckDir()
    {
        Vector3 vector = _ai.transform.position + new Vector3(_moveDir*2f, 0f, 0f);
        if (!Physics.Raycast(vector, Vector3.down, 1.5f) || Physics.Raycast(_ai.transform.position + new Vector3(0, _collider.center.y + _collider.size.y / 2, 0), Vector3.right * _moveDir, 1f))
            _moveDir *= -1f;
    }

    public void Update()
    {
        _movePosition();
    }

    void _movePosition()
    {
        CheckDir();
        Vector3 oldVel = _rigidbody.velocity;
        Vector3 vel;
        vel.y = oldVel.y;
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(_moveDir*speed) ? oldVel.x : _moveDir*speed;
        vel.z = 0;
        _rigidbody.velocity = vel * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_ai.transform.position.x + _moveDir, _ai.transform.position.y, _ai.transform.position.z) - _ai.transform.position);
        _ai.transform.rotation = Quaternion.Slerp(_ai.transform.rotation, targetRotation, 5f * Time.deltaTime);
    }
}
