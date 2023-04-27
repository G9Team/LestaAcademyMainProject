using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace New
{

    public class PlayerData : IPlayerData
    {
        public int MaxHealth { get; private set; }
        public int AttackForce { get; private set; }
        private int _maxEnergy;
        private int _currentLevel;

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

        private int _currrentHealth = 3;

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
                    OnHealthToZero?.Invoke();
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
        private Dictionary<int, int> _starsByLevel = new Dictionary<int, int>();
        private bool _isVulnerable = true;
        public PlayerData(int maxLevelIndex, int currentLevelIndex)
        {
            foreach (var value in Enum.GetValues(typeof(CollectableType)))
            {
                _itemAmountByType.Add((CollectableType)value, 0);
            }
            _currentLevel = currentLevelIndex;
            for (int i = 0; i < maxLevelIndex; i++)
            {
                _starsByLevel.Add(i, 0);
            }
            _isVulnerable = true;
        }

        public void SetLevel(int currentLevelIndex){
            _currentLevel = currentLevelIndex;
        }

        public void ChangeHealth(int health){
            if(health < 0 && !_isVulnerable) return;
            if (health < 0){
                Invulnerability();
            }
            CurrentHealth += health;
        }

        async private void Invulnerability(){
            _isVulnerable = false;
            await System.Threading.Tasks.Task.Run(() =>{
                Thread.Sleep(1000);
                _isVulnerable = true;
            } );
            
        }

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

        public void LoadSave(SaveManager.SaveObject saveObject)
        {
            MaxHealth = saveObject.maxHealth;
            AttackForce = saveObject.attackForce;
            _maxEnergy = saveObject.maxEnergy;
        }


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

                case UpgradeType.Star:
                    _starsByLevel[_currentLevel] += valueToUpgrade;
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
            SaveManager.AddValue(typeOfUpgrade, valueToUpgrade);
        }

        void IPlayerData.AddCollectable(CollectableType type) =>
            _itemAmountByType[type]++;

        void IPlayerData.AddCollectable(CollectableType type, int vlaue) =>
            _itemAmountByType[type] += vlaue;
    }

}
