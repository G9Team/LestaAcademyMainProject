using System;
using UnityEngine;


namespace New
{


    public class WallDetetor : MonoBehaviour, IDetector
    {
        public DetectionType TypeOfDetection { get; } = DetectionType.Wall;

        public event Action<DetectionType, bool> OnDetectionApear;

        [SerializeField] private Transform _forward;
        [SerializeField] private LayerMask _everythingButPlayerAndEnemy;
        private bool _wallDetected = false;


        void Update()
        {
            IsForwardWallColidied();
        }

        private void IsForwardWallColidied()
        {
            bool walled = _wallDetected;
            _wallDetected = false;
            for (int i = 0; i < _forward.childCount; i++)
            {
                if (Physics.CheckSphere(_forward.GetChild(i).position, 0.05f, _everythingButPlayerAndEnemy) == true)
                {
                    _wallDetected = true;

                }
            }
            if (_wallDetected != walled)
            {
                OnDetectionApear.Invoke(TypeOfDetection, false);
            }
        }


    }
}
