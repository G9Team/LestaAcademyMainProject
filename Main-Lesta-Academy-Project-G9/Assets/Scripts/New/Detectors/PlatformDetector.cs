using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{


    public class PlatformDetector : MonoBehaviour, IDetector
    {

        [SerializeField] private LayerMask _platformCollision;
        public DetectionType TypeOfDetection { get; } = DetectionType.Platform;
        public event Action<DetectionType, bool> OnDetectionApear;
        /*private void CheckPlatform()
        {
            _platformCollision = Physics.OverlapSphere(groundChecker.position, _groundCheckRadius, platformLayer);
            if (_platformCollision.Length > 0) transform.SetParent(_platformCollision[0].transform);
            else transform.SetParent(null);

        }*/
    }
}
