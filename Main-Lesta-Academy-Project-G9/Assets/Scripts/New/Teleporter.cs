using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{
    public class Teleporter : MonoBehaviour
    {
        private Transform _positionToTeleport;
        void Start()
        {
            _positionToTeleport = transform.GetChild(0);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player"){
                other.transform.position = _positionToTeleport.position;
            }
        }
    }

}
