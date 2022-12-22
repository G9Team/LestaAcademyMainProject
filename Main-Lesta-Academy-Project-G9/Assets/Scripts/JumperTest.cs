using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperTest : MonoBehaviour
{
    [SerializeField] private Transform _legsPosition;
    [SerializeField] private float _jumpForce, _mainGravityScaler;
    public LayerMask EverythingAceptPlayer;
    private Rigidbody _rigidBody;
    private bool _dubleJump = false, _isJumping, _checker;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Jump();
    }
    private void FixedUpdate()
    {

        _rigidBody.velocity += Vector3.up * _rigidBody.mass * _mainGravityScaler * -1;
    }
    private bool IsGrounded()
    {
        bool result = Physics.CheckSphere(_legsPosition.position, 0.05f, EverythingAceptPlayer) ||
                      Physics.CheckSphere(_legsPosition.GetChild(0).position, 0.05f, EverythingAceptPlayer);
        if (result){
            _dubleJump = true;
            _checker = true;
        }
        return  result;
    }
    private void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && (_dubleJump || IsGrounded()))
        {
            _checker = true;
            float tempVelocity = _rigidBody.velocity.x;
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.velocity += Vector3.up * _jumpForce * _mainGravityScaler * _mainGravityScaler;
            _rigidBody.velocity += new Vector3(tempVelocity, 0, 0);
            if (IsGrounded()) return;
            else if (_dubleJump)
            {
                _checker = false;
                _dubleJump = false;
                return;
            }
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (_checker) return;
        if (other.collider.bounds.max.y < GetComponent<Collider>().bounds.min.y) 
        {
            IsGrounded();
        }
        Debug.Log($"Checking doublejump: {_dubleJump}, Checker: {_checker}, IsGrounded: {IsGrounded()}");
    }

}
