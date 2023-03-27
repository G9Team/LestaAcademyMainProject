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
        public event Action<DetectionType> OnDetectionApear;
        private bool _checkGrounded = true;

        // Update is called once per frame
        private void OnCollisionStay(Collision other)
        {
            if(!_checkGrounded) {return;}

            CheckGrounded();
        }

        private void OnCollisionExit(Collision other) => _checkGrounded = true;

        private void CheckGrounded()
        {
            if (Physics.CheckSphere(_legsPosition.position, 0.05f, _everythingButPlayer) ||
                Physics.CheckSphere(_legsPosition.GetChild(0).position, 0.05f, _everythingButPlayer))
            {
                _checkGrounded = false;
                OnDetectionApear.Invoke(TypeOfDetection);
            }
            
        }


    }
}
