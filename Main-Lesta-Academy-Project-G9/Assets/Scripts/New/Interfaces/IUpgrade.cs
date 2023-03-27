using System;

namespace New
{
    public interface IUpgrade
    {
        public UpgradeType Type {get;}
        public float UpgradeValue {get;}

    }

    public interface IUpgrader
    {
        public IPlayerData DataToUpgrade {get;}
        public void StartUpgrade(IUpgrade upgrade);
    }



}