using System;

namespace New
{

    public class PlayerUpgrader : IUpgrader
    {
        public  IPlayerData DataToUpgrade {get; private set;}

        public PlayerUpgrader(IPlayerData playerData)
        {
            this.DataToUpgrade = playerData;
        }
        public void StartUpgrade(IUpgrade upgrade){
            if (upgrade.Type == UpgradeType.Damage){
                DataToUpgrade.ChangeHealth((int) upgrade.UpgradeValue);
                return;
            }
            DataToUpgrade.Upgrade(upgrade.Type, (int) upgrade.UpgradeValue);
        }
    }
}