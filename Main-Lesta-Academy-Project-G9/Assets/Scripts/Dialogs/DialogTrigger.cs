using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogAsset asset;
    [SerializeField] private bool repeat;
    bool _activated;

    public void Activate()
    {
        if (_activated && !repeat) return;
        FindObjectOfType<DialogMain>().Activate(asset);
        _activated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Activate();
    }
}
