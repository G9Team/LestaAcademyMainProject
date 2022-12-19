using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AiStateBase
{
    AIBase _ai;
    Rigidbody _rigidbody;
    float _attackDistance = 1f;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _rigidbody = _ai.GetComponent<Rigidbody>();
    }

    public void MiniUpdate()
    {
        if (Vector3.Distance(_ai.transform.position, _ai._player.transform.position) <= _attackDistance)
            //change health here
            return;
    }

    public void Update()
    {
        if (Mathf.Abs(_ai.transform.position.y - _ai._player.transform.position.y) > 0.5f) return;
        _movePosition(_ai._player.transform.position);
    }

    void _movePosition(Vector3 position)
    {
        Vector3 oldVel = _rigidbody.velocity;
        Vector3 delta = position - _rigidbody.position;
        Vector3 vel = delta / Time.deltaTime;
        vel.y = oldVel.y;
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x;
        vel.z = 0f;
        _rigidbody.velocity = vel * Time.deltaTime;
    }
}
