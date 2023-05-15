using System;

namespace New
{

    public interface IDetector
    {
        public DetectionType TypeOfDetection {get;}
        public event Action<DetectionType, bool> OnDetectionApear;
    }

}