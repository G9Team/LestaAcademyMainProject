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
            _manager.DamagePlayer(health);
            RefreshHealth();
        }

        private void RefreshHealth() => _health = _playerData.CurrentHealth;

    }

}