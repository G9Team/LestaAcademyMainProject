using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AiStateBase
{
    AIBase _ai;

    public void Init(AIBase ai)
    {
        _ai = ai;
    }

    public void MiniUpdate()
    {
        //currently we dont have exact mechanics, so just switch to patrol
        if (Random.Range(0, 100) < 30)
        {
            _ai.ForceState(AIBase.ENEMY_STATE.PATROL);
        }
    }

    public void Update()
    {
    }
}
