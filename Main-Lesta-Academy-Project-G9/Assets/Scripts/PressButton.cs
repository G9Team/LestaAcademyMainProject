using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    private Animator buttonAnim;
    [SerializeField] Platform[] _platforms;

    private void Start()
    {
        buttonAnim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        buttonAnim.SetBool("IsPressed", true);
        if (_platforms.Length > 0)
        {
            foreach(Platform plat in _platforms)
            {
                plat.SayToMove();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        buttonAnim.SetBool("IsPressed", false);
    }
}
