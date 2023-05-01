using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{
    public class BossAttackHitbox : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerComponentManager>().DamagePlayer(-1);

            }
        }
        
    }

}
