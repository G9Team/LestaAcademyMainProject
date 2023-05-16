using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAI : AIBase
{

    ENEMY_STATE _enemyState;
    public AiStateBase _stateClass;
    [SerializeField] private float detectDistance;
    Animator _animator;

    public override void ForceState(ENEMY_STATE state)
    {
        _enemyState = state;
        _stateClass = null;
        string behaviour = "";
        switch (state)
        {
            case ENEMY_STATE.PATROL:
                behaviour = "PatrolState";
                break;
            case ENEMY_STATE.ATTACK:
                behaviour = "AttackNear";
                break;
            case ENEMY_STATE.IDLE:
                behaviour = "PatrolState";
                break;
        }
        if(behaviour != "")
            _stateClass = (AiStateBase)System.Activator.CreateInstance(System.Type.GetType(behaviour));
        if(_stateClass != null) _stateClass.Init(this);
    }

    public override string GetName()
    {
        return "Bear AI";
    }

    public override ENEMY_STATE GetState()
    {
        return _enemyState;
    }
    void Start()
    {
        Invoke("Init", 3f);
    }

    void Init()
    {
        _animator = GetComponent<Animator>();
        InitAI();
        ForceState(ENEMY_STATE.PATROL);
        InvokeRepeating("MiniUpdate", 1f, 1f);
    }

    //used for some events that not needed to call every frame
    void MiniUpdate()
    {
        if (health <= 0f) return;
        if (_stateClass != null) _stateClass.MiniUpdate();
        _enemy = null;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, 50f);
        foreach (Collider hit in hits)
        {
            IAlive alive = hit.GetComponent<IAlive>();
            if (alive != null)
            {
                if(Mathf.Abs(hit.transform.position.x-this.transform.position.x) > detectDistance)
                    continue;
                if (_enemy == null)
                    _enemy = alive;
                else
                {
                    if (Mathf.Abs(this.transform.position.x-GetEnemyPosition().x) > Mathf.Abs(hit.transform.position.x-this.transform.position.x))
                        _enemy = alive;
                }
            }
        }
        if(_enemy != null && _enemyState != ENEMY_STATE.ATTACK)
            ForceState(ENEMY_STATE.ATTACK);
        else if(_enemy == null && _enemyState == ENEMY_STATE.ATTACK)
            ForceState(ENEMY_STATE.IDLE);
    }

    void Update()
    {
        if (health <= 0f) return;
        UpdateAI();
        if (_stateClass != null) _stateClass.Update();
    }

    public override void OnTakeDamage(float damage)
    {
        if (health <= 0f) return;
        health -= damage;
        if (health <= 0)
        {
            _stateClass = null;
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<BoxCollider>());
            Destroy(GetComponent<CapsuleCollider>());
            _animator.SetTrigger("death");
            Destroy(this.gameObject, 5f);
        }
        else
        {
            _animator.SetTrigger("takeDamage");
        }
    }
}
