using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{

    public class LedgeDetector : MonoBehaviour, IDetector
    {
        public DetectionType TypeOfDetection {get;} = DetectionType.Ledge;

        public event Action<DetectionType> OnDetectionApear;


        // Implement ledge detection logic!
    }


}
