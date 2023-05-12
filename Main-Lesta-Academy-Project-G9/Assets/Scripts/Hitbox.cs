using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    FootstepSound fSound;
    void Start()
    {
        fSound = this.transform.parent.GetComponent<FootstepSound>();
    }


    void OnCollisionEnter()
    {
        fSound.PlayHit();
    }

    void OnTriggerEnter(Collider col)
    {
        fSound.PlayHit();
        AIBase ai = col.gameObject.GetComponent<AIBase>();
        if (ai != null)
        {
            ai.OnTakeDamage(10f);
        }
    }
}
