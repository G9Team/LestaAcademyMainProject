using System;

namespace New
{

    public enum PlayerState
    {
        Idle = 0,
        Run,
        Jump,
        Hang,
        PullUp,
        TakeDamage,
        Interact,
        PullBox,
        PushBox,

        Falling, // USe unity State machine
        

        Attack = 40
    }

}