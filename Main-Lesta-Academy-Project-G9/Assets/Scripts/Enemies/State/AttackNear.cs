using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNear : AiStateBase
{
    AIBase _ai;
    Rigidbody _rigidbody;
    public float attackDistance = 2f;
    bool _attacking = false;
    Animator _animator;
    public bool dontMoveWhileAttacking = false;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _rigidbody = _ai.GetComponent<Rigidbody>();
        _animator = _ai.GetComponent<Animator>();
    }

    public void MiniUpdate()
    {
        bool needAttack = Vector3.Distance(_ai.transform.position, _ai.GetEnemyPosition()) <= attackDistance;
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
        if (Mathf.Abs(_ai.transform.position.y - enemyPos.y) > 2f) return;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(enemyPos.x, _ai.transform.position.y, enemyPos.z) - _ai.transform.position);
        _ai.transform.rotation = Quaternion.Slerp(_ai.transform.rotation, targetRotation, 5f * Time.deltaTime);
        
    }

    public void FixedUpdate()
    {
        Vector3 enemyPos = _ai.GetEnemyPosition();
        if (Mathf.Abs(_ai.transform.position.y - enemyPos.y) > 2f) return;
        _movePosition(enemyPos);
    }

    void _movePosition(Vector3 position)
    {
        Vector3 oldVel = _rigidbody.velocity;
        Vector3 delta = position - _rigidbody.position;
        Vector3 vel = delta / Time.deltaTime;
        vel.y = oldVel.y * Physics.gravity.y*Time.deltaTime;
        vel.x = Mathf.Min(Mathf.Max((Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x), -1f), 1f)*200;
        vel.z = 0f;
        if (!(_attacking && dontMoveWhileAttacking))
            _rigidbody.velocity = vel * Time.deltaTime;
        
    }
}
