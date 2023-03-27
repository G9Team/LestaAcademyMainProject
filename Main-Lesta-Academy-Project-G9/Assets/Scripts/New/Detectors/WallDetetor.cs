using System;
using UnityEngine;


namespace New
{


    public class WallDetetor : MonoBehaviour, IDetector
    {
        public DetectionType TypeOfDetection { get; } = DetectionType.Wall;

        public event Action<DetectionType> OnDetectionApear;

        [SerializeField] private Transform _forward;
        [SerializeField] private LayerMask _everythingButPlayerAndEnemy;
        private bool _wallDetected = false;


        void Update()
        {
            IsForwardWallColidied();
        }

        private void IsForwardWallColidied()
        {
            for (int i = 0; i < _forward.childCount; i++)
            {
                if (Physics.CheckSphere(_forward.GetChild(i).position, 0.05f, _everythingButPlayerAndEnemy) != _wallDetected)
                {
                    OnDetectionApear.Invoke(TypeOfDetection);
                    _wallDetected = Physics.CheckSphere(_forward.GetChild(i).position, 0.05f, _everythingButPlayerAndEnemy);
                    
                }
            }
        }


    }
}
