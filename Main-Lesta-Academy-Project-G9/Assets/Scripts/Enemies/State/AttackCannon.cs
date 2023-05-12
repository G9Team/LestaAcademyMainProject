using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCannon : AiStateBase
{
    AIBase _ai;
    public float attackDistance = 15f;
    Animator _animator;
    AudioSource _source;
    GameObject _projectile;

    public void Init(AIBase ai)
    {
        _ai = ai;
        _source = _ai.GetComponent<AudioSource>();
        _animator = _ai.GetComponent<Animator>();
        _projectile = Resources.Load<GameObject>("CannonProjectile");
    }

    public void MiniUpdate()
    {
        if (_ai._enemy == null) return;
        bool needAttack = Vector3.Distance(_ai.transform.position, _ai.GetEnemyPosition()) <= attackDistance;
        _animator.SetBool("attack", needAttack);
    }

    public void Update()
    {
    }

    public void Attack()
    {
        GameObject spawned = GameObject.Instantiate(_projectile, _ai.transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity);
        CannonProjectile cp = spawned.GetComponent<CannonProjectile>();
        cp.throwType = CannonProjectile.type.CURVE;
        cp.lifeTime = 5f;
        if (_source != null)
            _source.Play();
    }
}
