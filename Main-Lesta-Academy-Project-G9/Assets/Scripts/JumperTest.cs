using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperTest : MonoBehaviour
{
    [SerializeField] private Transform _legsPosition;
    [SerializeField] private float _jumpForce, _timeToCheck, _mainGravityScaler;
    //[SerializeField] private float _gravityIncreaser;
    public LayerMask EverythingAceptPlayer;
    private Rigidbody _rigidBody;
    private bool _doCheckGround = true, _dubleJump = false;
    private WaitForSeconds _wait;

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody>();
        _wait = new WaitForSeconds(_timeToCheck);
    }
    private void Update() {
        Jump();
    }
    private void FixedUpdate() {
       
        //VelocityHandler();
        _rigidBody.velocity += Vector3.up * _rigidBody.mass * _mainGravityScaler * -1;
    }
    private bool IsGrounded(){
        return _doCheckGround && Physics.CheckSphere(_legsPosition.position, 0.05f, EverythingAceptPlayer);
    }
    private void Jump(){

        if (Input.GetKeyDown(KeyCode.Space) && ( _dubleJump || IsGrounded())){
            float tempVelocity = _rigidBody.velocity.x;
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.velocity += Vector3.up * _jumpForce * _mainGravityScaler * _mainGravityScaler;
            _rigidBody.velocity += new Vector3(tempVelocity, 0, 0);
            Debug.Log($"Jumping {_dubleJump}, {IsGrounded()}");
            if (_dubleJump){
                _dubleJump = false;
                return;
            }
            if (IsGrounded()) _dubleJump = true;
            StartCoroutine(TemporaryDontCheckGround());
        }
    }

    private IEnumerator TemporaryDontCheckGround(){
        _doCheckGround = false;
        yield return _wait;
        _doCheckGround = true;
    }
    /*
    private void VelocityHandler(){
        if (_rigidBody.velocity.y < 0){
           // _rigidBody.velocity += Vector3.up * _gravityIncreaser * -1;
        }
    }
    */
}
