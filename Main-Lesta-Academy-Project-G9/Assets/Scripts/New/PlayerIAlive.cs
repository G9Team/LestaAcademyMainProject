using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{

    public class PlayerIAlive : MonoBehaviour, IAlive
    {
        private IPlayerData _playerData;
        private PlayerComponentManager _manager;
        private int _health = 0;

        private void Start()
        {
            _manager = GetComponent<PlayerComponentManager>();
            _playerData = _manager.GetPlayerData();
            _health = _playerData.CurrentHealth;
        }

        public int GetHealth() => _health;

        public void SetHealth(int health)
        {
            _playerData.SetHealth(health);
            RefreshHealth();
        }

        public void ChangeHealth(int health)
        {
            _manager.DamagePlayerFromEnemy(CalculateDirection(), health);
            RefreshHealth();
        }
        
        private float CalculateDirection(){
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closestEnemy = null;
            foreach (var enemy in enemies)
            {
                if (closestEnemy is null 
                    || Vector3.Distance(this.transform.position, closestEnemy.transform.position) > 
                    Vector3.Distance(this.transform.position, enemy.transform.position))
                {
                    closestEnemy = enemy;
                } 
            }
            return Mathf.Sign(this.transform.position.x - closestEnemy.transform.position.x);
        }

        private void RefreshHealth() => _health = _playerData.CurrentHealth;

    }

}