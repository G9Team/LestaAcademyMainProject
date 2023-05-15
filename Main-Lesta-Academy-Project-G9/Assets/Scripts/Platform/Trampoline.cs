using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using New;


public class Trampoline : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    private VelocityHandler _playerVelocity;
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player"){
            _playerVelocity = other.GetComponent<PlayerComponentManager>().GetVelocityHandler();
            _animator.Play("OnJump");
        }
    }


    public void ImplementVelocity(){
        _playerVelocity.Trampoine(_jumpForce);
        _playerVelocity.gameObject.GetComponent<Animator>().Play("Falling");
        _playerVelocity = null;
    }
    

}
