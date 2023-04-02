using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFly : AiStateBase
{
    AIBase _ai;
    Rigidbody _rigidbody;
    float _attackDistance = 2f;
    bool _attacking = false;
    Animator _animator;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _rigidbody = _ai.GetComponent<Rigidbody>();
        _animator = _ai.GetComponent<Animator>();
    }

    public void MiniUpdate()
    {
        bool needAttack = Mathf.Abs(_ai.transform.position.x-_ai.GetEnemyPosition().x) <= _attackDistance;
        if (needAttack != _attacking)
        {
            _attacking = needAttack;
            _animator.SetBool("attack", _attacking);
            _ai._enemy.ChangeHealth(-1);
        }
    }

    public void Update()
    {
        Vector3 enemyPos = _ai.GetEnemyPosition();
        _movePosition(enemyPos);
    }

    void _movePosition(Vector3 position)
    {
        Vector3 oldVel = _rigidbody.velocity;
        Vector3 delta = position - _rigidbody.position;
        Vector3 vel = delta / Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(_ai.transform.position, Vector3.down, out hit, 100f))
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
        vel.z = 0f;
        _rigidbody.velocity = vel * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(_ai.transform.position - new Vector3(position.x, _ai.transform.position.y, position.z));
        _ai.transform.rotation = Quaternion.Slerp(_ai.transform.rotation, targetRotation, 5f * Time.deltaTime);
    }
}
