using System;
using UnityEngine;

namespace New
{

    public interface IPlayerData : IPlayerBattleData, IUpgradable
    {
        public int Wheet { get; }
        public int Pirojki { get; }
        public int Coins { get; }

        public int GetCollectableCount(CollectableType type);
        public void AddCollectable(CollectableType type);
        public void AddCollectable(CollectableType type, int vlaue);
        public void SetLevel(int currentLevelIndex);


    }
    
    public interface IPlayerBattleData : IAttack, IAlive
    {
        public int MaxHealth { get; }
        public int MaxEnergy { get; }
        public int CurrentHealth { get; }
        public int CurrentEnergy { get; }
        public Vector3 KnockbackForce { get; }
        public event Action OnHealthToZero, OnUIUpdate;
    }


    public interface IAttack
    {
        public int AttackForce { get; }
    }
}