using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{

    public class GroundDetector : MonoBehaviour, IDetector
    {
        [SerializeField] private Transform _legsPosition;
        [SerializeField] private LayerMask _everythingButPlayer;
        public DetectionType TypeOfDetection { get; } = DetectionType.Ground;
        public event Action<DetectionType, bool> OnDetectionApear;
        private bool _checkGrounded = true;

        // Update is called once per frame
        private void OnCollisionStay(Collision other)
        {
            if(!_checkGrounded) {return;}

            CheckGrounded();
        }

        private void OnCollisionExit(Collision other) => CheckGrounded();
        private void OnTriggerExit(Collider other) { CheckGrounded();}

        private void CheckGrounded()
        {
            bool tmp = Physics.CheckSphere(_legsPosition.position, 0.05f, _everythingButPlayer) ||
                Physics.CheckSphere(_legsPosition.GetChild(0).position, 0.05f, _everythingButPlayer);
            _checkGrounded = !tmp;
            OnDetectionApear.Invoke(TypeOfDetection, tmp);
            
        }


    }
}
