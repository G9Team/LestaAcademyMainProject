using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoints : MonoBehaviour
{
    public event UnityAction<Vector3> OnPlayerCheckpoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Clision !");
            OnPlayerCheckpoint?.Invoke(other.transform.position);
        }
    }

}
