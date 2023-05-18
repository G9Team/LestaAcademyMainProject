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

        public DetectionType TypeOfDetection { get; } = DetectionType.HeavyInteraction;
        public event Action<DetectionType, bool> OnDetectionApear;

        private bool _isInitialized = false;

        public PlayerInteractor(IUpgrader upgrader, PlayerInteractionsChecker checker)
        {
            _upgrader = upgrader;
            _checker = checker;
            _isInitialized = true;
        }

        public void OnInteractionInput()
        {
            if (_checker.IsInteractionAvaliable())
            {
                ProceedInteraction(_checker.GetInteractable());
            }
            else
            {
                //some missclick logic, if necessary
            }
        }

        private void ProceedInteraction(IInteractable interactable)
        {
            ICollectable collectable = interactable.GetCollectable();
            if (interactable.IsUpgrade())
            {
                _upgrader.StartUpgrade(interactable.GetUpgrade());
            }
            else
            {
                //other interaction logic + OnDetectionApear.Invoke(TypeOfDetection);
            }

            collectable?.Collect();
            string clip = "";
            switch (interactable.GetCollectable().Type)
            {
                case UpgradeType.Star:
                    clip = "star";
                    break;
                case UpgradeType.CurrentHealth:
                    clip = "health";
                    break;
                case UpgradeType.CurrentEnergy:
                    clip = "energy";
                    break;
                case UpgradeType.Pirojki:
                    clip = "generalItem";
                    break;
                case UpgradeType.Wheet:
                    clip = "generalItem";
                    break;
            }
            if (clip != "")
                _checker.transform.parent.GetComponent<FootstepSound>().PlayCustom(clip);
        }


    }
}
