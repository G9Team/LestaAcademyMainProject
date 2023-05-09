using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace New
{
    public class BossHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        public int Health { get; private set; }
        public event UnityAction NextStage, Death, Damaged;


        private void Awake()
        {
            Health = _maxHealth;
        }
        public void GetDamage(int amount)
        {
            int nextHealth = Health - amount;
            if ((nextHealth < _maxHealth / 3 && Health >= _maxHealth / 3) ||
                (nextHealth < _maxHealth / 1.5 && Health >= _maxHealth / 1.5))
            {
                Debug.Log("Shift stage");
                NextStage?.Invoke();
                Health = nextHealth;
                return;

            }
            else if (nextHealth <= 0)
            {
                Death?.Invoke();
                Health = 0;
                return;
            }
            Damaged?.Invoke();
            Health = nextHealth;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerAtackHitbox")
            {
                IPlayerData data = other.transform.parent.GetComponent<PlayerComponentManager>().GetPlayerData();
                GetDamage(data.AttackForce);
            }
        }

    }

}
