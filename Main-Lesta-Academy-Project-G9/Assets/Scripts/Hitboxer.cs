using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitboxer : MonoBehaviour
{
    [SerializeField] private GameObject _hitbox;
    public void HitStarted() => _hitbox.SetActive(true);
    public void HitEnded() => _hitbox.SetActive(false);

}
