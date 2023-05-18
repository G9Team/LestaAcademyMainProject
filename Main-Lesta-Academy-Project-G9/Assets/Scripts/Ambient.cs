using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : MonoBehaviour
{
    public AudioClip[] ambientSounds;
    AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        Invoke("InvokeSound", Random.Range(10f, 20f));
    }

    void InvokeSound()
    {
        AudioClip selected = ambientSounds[Random.Range(0, ambientSounds.Length)];
        _source.clip = selected;
        Debug.Log("ambient: " + _source.volume);
        _source.Play();

        Invoke("InvokeSound", selected.length+Random.Range(10f, 20f));
    }
}
