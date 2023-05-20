using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossCollectable : MonoBehaviour
{
   private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            SceneLoader.LoadScene(0);
        }
   }
}
