using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Killbox : MonoBehaviour
{
    public event UnityAction<GameObject> OnKillBoxColision;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player"){
            OnKillBoxColision?.Invoke(other.gameObject);
        }
    }
}
