using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNear : AiStateBase
{
    AIBase _ai;
    Rigidbody _rigidbody;
    float _attackDistance = 2f;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _rigidbody = _ai.GetComponent<Rigidbody>();
    }

    public void MiniUpdate()
    {
        if (Vector3.Distance(_ai.transform.position, _ai.GetEnemyPosition()) <= _attackDistance)
            _ai._enemy.ChangeHealth(-1);
    }

    public void Update()
    {
        Vector3 enemyPos = _ai.GetEnemyPosition();
        if (Mathf.Abs(_ai.transform.position.y - enemyPos.y) > 0.5f) return;
        _movePosition(enemyPos);
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
