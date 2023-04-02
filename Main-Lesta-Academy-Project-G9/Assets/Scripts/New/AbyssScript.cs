using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssScript : MonoBehaviour
{
    [SerializeField] private RootCheckBox _checkPoints;

    private void Awake() {
        for (int i = 0; i < this.transform.childCount; i++){
            var child = this.transform.GetChild(i).GetComponent<Killbox>();
            if (child is null) { continue; }
            child.OnKillBoxColision += OnPlayerFall;
        }
    }
    private void OnPlayerFall(GameObject player){
        player.transform.position = _checkPoints.LastCheckpointPosition;
    }
}
