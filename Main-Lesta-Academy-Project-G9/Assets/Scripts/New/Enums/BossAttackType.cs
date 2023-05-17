using System;

namespace New
{
    public enum BossAttackType
    {
        #region FirstStage
        FireBall = 0,
        Spikes,
        #endregion

        #region SecondStage
        RailGun,
        FloorIsLava,
        #endregion

        #region ThirdStage
        BulletHell,
        #endregion
    }
}