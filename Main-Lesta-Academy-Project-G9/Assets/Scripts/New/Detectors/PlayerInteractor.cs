using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{

    public class PlayerInteractor : IDetector
    {
        private PlayerInteractionsChecker _checker;
        private IUpgrader _upgrader;

        public DetectionType TypeOfDetection {get;} = DetectionType.HeavyInteraction;
        public event Action<DetectionType> OnDetectionApear;

        private bool _isInitialized = false;

        public PlayerInteractor(IUpgrader upgrader, PlayerInteractionsChecker checker){
            _upgrader = upgrader;
            _checker = checker;
            _isInitialized = true;
        }

        public void OnInteractionInput(){
            if (_checker.IsInteractionAvaliable()){
                ProceedInteraction(_checker.GetInteractable());
            }
            else{
                //some missclick logic, if necessary
            }
        }

        private void ProceedInteraction(IInteractable interactable){
            if (interactable.IsUpgrade()){
                _upgrader.StartUpgrade(interactable.GetUpgrade());
            }
            else {
                //other interaction logick + OnDetectionApear.Invoke(TypeOfDetection);
            }
        }


    }
}
