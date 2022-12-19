using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AiStateBase
{
    void Init(AIBase ai);
    void MiniUpdate();
    void Update();
}
