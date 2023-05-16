using System;
using System.Collections;
using UnityEngine;

namespace New
{

    public class VelocityHandler : MonoBehaviour, IMover
    {
        [SerializeField]
        private float _jumpForce, _mainGravityScaler, _fallGravityScaler,
            _dashForce = 15f, _dashDuration = 0.25f, _runSpeed,
            _noControleDuraton, _xKnockbackForce, _yKnockbackForce;
        private bool _isFacingRight, _isAttacking, _isAttacked;
        private float _move
        {
            get
            {
                return _internalMove;
            }
            set
            {
                _internalMove = value * _moveScaler;
            }
        }
        private float _internalMove, _moveScaler = 1, _currentAttackSpeedX;
        
        private int _direction = -1;

        private Rigidbody _rigidbody;
        public bool IsDashing { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update() {
            if(_isAttacked){
                Attacked(_xKnockbackForce, _yKnockbackForce);  

            }
        }
        private void FixedUpdate()
        {
            if (IsDashing)
            {
                Dashing();
                return;
            }
            if (_isAttacked){
                GravityHandler();

                return;
            }         

            _rigidbody.velocity = new Vector3(_move * (_runSpeed), _rigidbody.velocity.y, 0);
            _direction = _move != 0 ? (int)Mathf.Sign(_move) : _direction;
            if (_move > 0 && !_isFacingRight) Flip(_move);
            else if (_move < 0 && _isFacingRight) Flip(_move);

            GravityHandler();
        }

        public void Move(float direction)
        {
            _move = direction;

        }

        public void Jump()
        {

            float tempVelocity = _rigidbody.velocity.x;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.velocity += Vector3.up * _jumpForce * _mainGravityScaler * _mainGravityScaler;
            _rigidbody.velocity += new Vector3(tempVelocity, 0, 0);

        }

        public void Dash()
        {
            if (!IsDashing)
            {
                StartCoroutine(DashTimer());
            }
        }
        
        public void Trampoine(float trampolineForce){
            _rigidbody.velocity = new Vector3(
                0, 
                trampolineForce * _jumpForce * _mainGravityScaler * _mainGravityScaler,
                0
            );
        }

        private void Dashing()
        {
            Vector3 dashspeed = new Vector3(_dashForce * _direction, 0, 0);
            _rigidbody.velocity = dashspeed;

        }
        private IEnumerator DashTimer()
        {
            IsDashing = true;
            yield return new WaitForSeconds(_dashDuration);
            IsDashing = false;
        }


        public void ChangeSpeed(float speedScale)
        {
            _moveScaler = speedScale;
        }


        private void GravityHandler()
        {
            if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.velocity += Vector3.up * _fallGravityScaler;
            }
            _rigidbody.velocity += Vector3.up * _rigidbody.mass * _mainGravityScaler * -1;
        }

        private void Flip(float moveInput)
        {
            _isFacingRight = !_isFacingRight;
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y + 180f * Mathf.Sign(moveInput),
                transform.localEulerAngles.z
                );
        }
        public void StopAttack() => _isAttacking = false;

        private void AttackedMovement(){
        }
        public void Attacked(){
            _isAttacked = true;
        }

        public void Attacked(float knockbackX, float knockbackY){
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.velocity = new Vector3 (knockbackX * _direction * -1, knockbackY, 0);
            StartCoroutine(NoControllTime());
            
        }
        private IEnumerator NoControllTime(){
            yield return new WaitForSeconds(_noControleDuraton);
            _isAttacked = false;
        }

    }


}