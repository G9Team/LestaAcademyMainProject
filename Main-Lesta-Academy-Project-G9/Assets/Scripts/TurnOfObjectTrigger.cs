using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOfObjectTrigger : MonoBehaviour
{
    private enum ObjectMode{
        SwitchActive,
        Delete,
        Replase
    }
    [SerializeField] private ObjectMode _objectMode;
   [SerializeField] private GameObject _objectToTurn, _objectToReplase;

   private void OnTriggerEnter(Collider other) {
    if(other.tag == "Player" && _objectToTurn is not null){
        switch (_objectMode)
        {
            case ObjectMode.SwitchActive:
                _objectToTurn.SetActive(!_objectToTurn.activeSelf); 
                break;
            case ObjectMode.Delete:
                Destroy(_objectToTurn);
                break;
            case ObjectMode.Replase:
                Instantiate(_objectToReplase, _objectToTurn.transform.position, Quaternion.identity);
                Destroy(_objectToTurn);
                break;
        }
    }
   }
   
}
