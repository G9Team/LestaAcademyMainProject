using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExplode : AiStateBase
{
    AIBase _ai;
    Rigidbody _rigidbody;
    float _activateDistance = 2f;
    Coroutine _explosionCoroutione;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _rigidbody = _ai.GetComponent<Rigidbody>();
    }

    public void MiniUpdate()
    {
        if (Vector3.Distance(_ai.transform.position, _ai.GetEnemyPosition()) <= _activateDistance)
        {
            if (_explosionCoroutione == null)
                _explosionCoroutione = _ai.StartCoroutine(explode());
        }
        else if(_explosionCoroutione != null)
        {
            _ai.StopCoroutine(_explosionCoroutione);
            _explosionCoroutione = null;
        }    
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(1f);
        _ai._enemy.ChangeHealth(-2);
        GameObject.Destroy(_ai.gameObject);
    }



    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        Vector3 enemyPos = _ai.GetEnemyPosition();
        if (Mathf.Abs(_ai.transform.position.y - enemyPos.y) > 0.5f) return;
        _movePosition(enemyPos);
    }

    void _movePosition(Vector3 position)
    {
        Vector3 oldVel = _rigidbody.velocity;
        Vector3 delta = position - _rigidbody.position;
        Vector3 vel = delta / Time.fixedDeltaTime;
        vel.y = oldVel.y;
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x;
        vel.z = 0f;
        _rigidbody.velocity = vel * Time.fixedDeltaTime;
    }
}
