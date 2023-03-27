using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{

    public class PlayerData : IPlayerData
    {
        public int MaxHealth { get; private set; }
        public int AttackForce { get; private set; }
        private int _maxEnergy;

        private int _currrentEnergy;
        public int CurrentEnergy
        {
            get
            {
                return _currrentEnergy;
            }
            private set
            {
                _currrentEnergy = (value > _maxEnergy) ? _maxEnergy :
                                  (value < 0) ? 0 : value;
            }
        }

        private int _currrentHealth;

        public int CurrentHealth
        {
            get
            {
                return _currrentHealth;
            }
            private set
            {
                if (value > MaxHealth)
                {
                    _currrentHealth = MaxHealth;
                }
                else if (value <= 0)
                {
                    _currrentHealth = 0;
                    OnHealthToZero.Invoke();
                }
                else
                {
                    _currrentHealth = value;
                }

            }
        }

        private float _knockbackForce = 40f;
        public Vector3 KnockbackForce // need Direction to multiply!
        {
            get
            {
                return Vector3.right * _knockbackForce;
            }
        }

        public event Action OnHealthToZero;

        private Dictionary<CollectableType, int> _itemAmountByType = new Dictionary<CollectableType, int>();

        public void ChangeHealth(int health) =>
            CurrentHealth += health;

        public int GetCollectableCount(CollectableType typeOfItem)
        {
            if (_itemAmountByType.ContainsKey(typeOfItem))
            {
                return _itemAmountByType[typeOfItem];
            }
            return 0;
        }

        public int GetHealth() => CurrentHealth;

        public void SetHealth(int health) =>
            CurrentHealth = (health > MaxHealth) ? MaxHealth : (health <= 0) ? 0 : health;


        void IUpgradable.Upgrade(UpgradeType typeOfUpgrade, float valueToUpgrade) { return; }


        void IUpgradable.Upgrade(UpgradeType typeOfUpgrade, int valueToUpgrade)
        {

            switch (typeOfUpgrade)
            {
                case UpgradeType.Wheet:
                    _itemAmountByType[(CollectableType)typeOfUpgrade] += valueToUpgrade;
                    break;

                case UpgradeType.Pirojki:
                    _itemAmountByType[(CollectableType)typeOfUpgrade] += valueToUpgrade;
                    break;

                case UpgradeType.MaxHealth:
                    MaxHealth += valueToUpgrade;
                    break;

                case UpgradeType.MaxEnergy:
                    _maxEnergy += valueToUpgrade;
                    break;

                case UpgradeType.CurrentHealth:
                    CurrentHealth += valueToUpgrade;
                    break;

                case UpgradeType.CurrentEnergy:
                    CurrentEnergy += valueToUpgrade;
                    break;

                case UpgradeType.AttackForce:
                    AttackForce += valueToUpgrade;
                    break;

                default:
                    return;
            }
        }

        void IPlayerData.AddCollectable(CollectableType type) =>
            _itemAmountByType[type]++;

        void IPlayerData.AddCollectable(CollectableType type, int vlaue) =>
            _itemAmountByType[type] += vlaue;
    }

}
