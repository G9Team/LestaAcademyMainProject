using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCheckBox : MonoBehaviour
{
    public Vector3 LastCheckpointPosition { get; private set; }

    private void Awake() {
        for (int i = 0; i < this.transform.childCount; i++){
            var child = this.transform.GetChild(i).GetComponent<CheckPoints>();
            if (child is null) { continue; }
            child.OnPlayerCheckpoint += SaveCheckpoint;
        }
    }
    private void SaveCheckpoint (Vector3 toSave){
        LastCheckpointPosition = toSave;
    }

}
