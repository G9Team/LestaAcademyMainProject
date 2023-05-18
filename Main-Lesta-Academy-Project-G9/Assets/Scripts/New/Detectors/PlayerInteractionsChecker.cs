using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace New
{

    public class PlayerInteractionsChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactableOnly;
        [SerializeField] private float _checkRdius;
        [SerializeField] private GameObject _hint;
        public bool IsInteractionAvaliable()
        {
            if (Physics.CheckSphere(this.transform.position, _checkRdius, _interactableOnly)){
                return true;
            }
            return false;
        }

        public IInteractable GetInteractable()
        {
            Collider[] interactableWithinRadius = Physics.OverlapSphere(this.transform.position, _checkRdius, _interactableOnly);
            return interactableWithinRadius[0].GetComponent<IInteractable>();

        }

        private void Update() {
            if (Physics.CheckSphere(this.transform.position, _checkRdius, _interactableOnly)){
                _hint.SetActive(true);
            }else{
                _hint.SetActive(false);
            }
        }
    }



}
