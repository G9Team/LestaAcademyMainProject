using System;

namespace New
{

    public interface IMover
    {
        public void Move (float direction);
        public void Jump();
        public void Dash();
        public void StopAttack();
        
        //<summary>
        //Method ChangeSpeed is to set scale of character speed
        //</summary>
        //<param name="speedScale"> Be shure, to not set to 0 or more, than 1.5 </param>
        public void ChangeSpeed(float speedScale);
    }

}