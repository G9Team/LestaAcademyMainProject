using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SceneLoader.ShowResult();
            other.GetComponent<New.InputManager>().TurnOnUIInput();
        }
    }
}
