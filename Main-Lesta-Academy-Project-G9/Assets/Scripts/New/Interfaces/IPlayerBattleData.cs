using System;
using UnityEngine;

namespace New
{

    public interface IPlayerData : IPlayerBattleData, IUpgradable
    {
        public int GetCollectableCount(CollectableType type);
        public void AddCollectable(CollectableType type);
        public void AddCollectable(CollectableType type, int vlaue);

    }
    
    public interface IPlayerBattleData : IAttack, IAlive
    {
        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public int CurrentEnergy { get; }
        public Vector3 KnockbackForce { get; }
        public event Action OnHealthToZero;
    }

    public interface IUpgradable
    {
        public void Upgrade(UpgradeType typeOfUpgrade, float valueToUpgrade);
        public void Upgrade(UpgradeType typeOfUpgrade, int valueToUpgrade);
    }

    public interface IAttack
    {
        public int AttackForce { get; }
    }
}