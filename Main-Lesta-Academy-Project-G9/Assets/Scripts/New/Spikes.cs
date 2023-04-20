using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace New
{


    public class Spikes : MonoBehaviour
    {
        public event UnityAction<GameObject> OnSpikeDamage;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player"){
                OnSpikeDamage?.Invoke(other.gameObject);
            }
        }
    }
}