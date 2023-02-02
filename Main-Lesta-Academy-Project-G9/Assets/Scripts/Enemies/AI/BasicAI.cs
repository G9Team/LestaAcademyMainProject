using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : AIBase
{
    [System.Serializable]
    public struct BasicAIStates
    {
        public ENEMY_STATE state;
        public string behaviour;
    }

    [SerializeField] private BasicAIStates[] basicAIStates;

    ENEMY_STATE _enemyState;
    public AiStateBase _stateClass;
    [SerializeField] private float detectDistance;

    public override void ForceState(ENEMY_STATE state)
    {
        _enemyState = state;
        _stateClass = null;
        foreach(BasicAIStates aIStates in basicAIStates)
            if(aIStates.state == state)
            {
                _stateClass = (AiStateBase)System.Activator.CreateInstance(System.Type.GetType(aIStates.behaviour));
                break;
            }
        if(_stateClass != null) _stateClass.Init(this);
    }

    public override string GetName()
    {
        return "Basic AI";
    }

    public override ENEMY_STATE GetState()
    {
        return _enemyState;
    }
    void Start()
    {
        InitAI();
        ForceState(ENEMY_STATE.PATROL);
        InvokeRepeating("MiniUpdate", 1f, 1f);
    }

    //used for some events that not needed to call every frame
    void MiniUpdate()
    {
        if (_stateClass != null) _stateClass.MiniUpdate();
        _enemy = null;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, detectDistance);
        foreach (Collider hit in hits)
        {
            IAlive alive = hit.GetComponent<IAlive>();
            if (alive != null)
            {
                if (_enemy == null)
                    _enemy = alive;
                else
                {
                    if (Vector3.Distance(GetEnemyPosition(), this.transform.position) > Vector3.Distance(hit.transform.position, this.transform.position))
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
        UpdateAI();
        if (_stateClass != null) _stateClass.Update();
    }
}
