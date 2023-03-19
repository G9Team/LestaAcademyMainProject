using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperTest : MonoBehaviour
{
    [SerializeField] private Transform _legsPosition;
    [SerializeField] private float _jumpForce, _mainGravityScaler, _fallGravityScaler, _dashForce = 15f, _dashDuration = 0.25f;
    public LayerMask EverythingAceptPlayer;
    private Rigidbody _rigidBody;
    private bool _dubleJump = false, _isJumping, _grounded;
    private Animator _animator;
    public bool IsDashing {get; private set;}

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (IsDashing) return;
        Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            StartCoroutine(DashTimer());
        }
    }
    private void FixedUpdate()
    {
        if (IsDashing){
            Dashing();
            return;
        }
        GravityHandler();
    }
    private bool IsGrounded()
    {
        _grounded = Physics.CheckSphere(_legsPosition.position, 0.05f, EverythingAceptPlayer) ||
                      Physics.CheckSphere(_legsPosition.GetChild(0).position, 0.05f, EverythingAceptPlayer);
        if (_grounded){
            _dubleJump = true;
        }
        Debug.Log("ground chek");
        _animator.SetBool("grounded", _grounded);

        return  _grounded;
    }
    private void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || _dubleJump))
        {
            _animator.SetTrigger("jump");
            float tempVelocity = _rigidBody.velocity.x;
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.velocity += Vector3.up * _jumpForce * _mainGravityScaler * _mainGravityScaler;
            _rigidBody.velocity += new Vector3(tempVelocity, 0, 0);
            Debug.Log($"space input, grounded: {_grounded}, doublejump: {_dubleJump}");
            if (_grounded) {
                _grounded = false;
                return;
            }
            else if (_dubleJump)
            {
                _dubleJump = false;
                return;
            }
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (_grounded) return;
            IsGrounded();
        Debug.Log($"Checking doublejump: {_dubleJump}");
    }
    private void GravityHandler(){
        if (_rigidBody.velocity.y <0){
            _rigidBody.velocity += Vector3.up * _fallGravityScaler;
        }
        _rigidBody.velocity += Vector3.up * _rigidBody.mass * _mainGravityScaler * -1;

    }
    private void Dashing(){
        Vector3 dashspeed = new Vector3(_dashForce, 0, 0);
        _rigidBody.velocity = dashspeed;

    }
    private IEnumerator DashTimer(){
        IsDashing = true;
        yield return new WaitForSeconds (_dashDuration);
        IsDashing = false;
    }
}
