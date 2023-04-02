using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      AIBase ai = other.GetComponent<AIBase>();
      if(ai != null)
         ai.OnTakeDamage(100);
   }
}
