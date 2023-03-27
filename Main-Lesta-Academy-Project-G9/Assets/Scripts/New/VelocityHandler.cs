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
            _firstAttackSpeedX, _secondAttackSpeedX, _thirdAttackSpeedX;
        private bool _isFacingRight, _isAttacking;
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
        private int _direction;

        private Rigidbody _rigidbody;
        public bool IsDashing { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }


        private void FixedUpdate()
        {
            if (IsDashing)
            {
                Dashing();
                return;
            }
            if (_isAttacking){
                AttackMovement();
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
        public void FirstAttack()
        {
            _currentAttackSpeedX = _firstAttackSpeedX * _direction;
            _isAttacking = true;
            Debug.Log("first attack");
        }
        public void SecondAttack()
        {
            _currentAttackSpeedX = _secondAttackSpeedX * _direction;
            _isAttacking = true;
                        Debug.Log("second attack " + _isAttacking);

        }
        public void ThirdAttack()
        {
            _currentAttackSpeedX = _thirdAttackSpeedX * _direction;
            _isAttacking = true;
        }
        public void StopAttack() => _isAttacking = false;

        private void AttackMovement(){
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.velocity = new Vector3 (_currentAttackSpeedX, 0, 0);
        }

    }


}