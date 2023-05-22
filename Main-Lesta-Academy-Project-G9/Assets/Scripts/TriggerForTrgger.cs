using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForTrgger : MonoBehaviour
{
    [SerializeField] private TurnOfObjectTrigger _actualTrigger;
    [SerializeField] private New.UpgradeType _upgradeType;
    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Player"){
            New.IPlayerData data = other.GetComponent<New.PlayerComponentManager>().GetPlayerData();
            int intToCheck = _upgradeType == New.UpgradeType.Wheet ? data.Wheet : data.Pirojki;
            if (intToCheck >= 10) {
                _actualTrigger.ProceedAction(other);
                Destroy(this.gameObject);
            }
        }
    }
}
