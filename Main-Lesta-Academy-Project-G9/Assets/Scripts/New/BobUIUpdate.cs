using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{
public class BobUIUpdate : MonoBehaviour
{
    public bool IsHeartActive {get; private set; } = true;
    public bool IsEnergyActive {get; private set; } = true;

    private const int MAX_UPDATE_VALUE = 7;
    private IPlayerData _data;
    private UpgradeType _upgradeType = UpgradeType.MaxEnergy, _subType = UpgradeType.CurrentEnergy;
    
    private void Awake() {
        _data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>().GetPlayerData();
        IsEnergyActive = _data.MaxEnergy < MAX_UPDATE_VALUE;
        IsHeartActive = _data.MaxHealth < MAX_UPDATE_VALUE;
    }

    public void OnHeartPick(){
        if(_data.MaxHealth == MAX_UPDATE_VALUE){
            //proceed disable
            return;
        }
        _upgradeType = UpgradeType.MaxHealth;
        _subType = UpgradeType.CurrentHealth;
    }
    public void OnEnergyPich(){
            if(_data.MaxEnergy == MAX_UPDATE_VALUE){
            //proceed disable
            return;
        }
        _upgradeType = UpgradeType.MaxEnergy;
        _subType = UpgradeType.CurrentEnergy;
    }

    public void ProceedUpdate(){
        _data.Upgrade(_upgradeType, 1);
        _data.Upgrade(_subType, 1);
        Destroy(this.gameObject);
    }
 
    }
}
    

