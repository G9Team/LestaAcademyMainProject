using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : AIBase
{
    ENEMY_STATE _enemyState;
    AiStateBase _stateClass;

    public override void ForceState(ENEMY_STATE state)
    {
        _enemyState = state;
        switch(state)
        {
            case ENEMY_STATE.PATROL:
                _stateClass = new PatrolState();
                break;
            case ENEMY_STATE.ATTACK:
                _stateClass = new AttackState();
                break;
            case ENEMY_STATE.IDLE:
                _stateClass = new IdleState();
                break;
            case ENEMY_STATE.INACTIVE:
                _stateClass = null;
                break;
        }
        if(_stateClass != null) _stateClass.Init(this);
    }

    public override string GetEnemyName()
    {
        return "Test AI";
    }

    public override ENEMY_STATE GetState()
    {
        return _enemyState;
    }
    void Start()
    {
        canDetectThroughWalls = false;
        InitAI();
        ForceState(ENEMY_STATE.PATROL);
        InvokeRepeating("MiniUpdate", 1f, 1f);
    }

    //used for some events that not needed to call every frame
    void MiniUpdate()
    {
        if (_stateClass != null) _stateClass.MiniUpdate();
    }

    void Update()
    {
        UpdateAI();
        if (_stateClass != null) _stateClass.Update();
    }
}
