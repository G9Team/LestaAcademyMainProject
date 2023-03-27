using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{

    public class PlayerAnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject _hitbox;

        private bool _isGrounded = true, _isRunning, _isInteracting, _combo, _isAttacking, _canSetCombo;

        public void SetGrounded(bool groundedToSet) => _isGrounded = groundedToSet;
        public void SetRun(bool runToSet) => _isRunning = runToSet;
        public void SetInteracting(bool interacting) => _isInteracting = interacting;
        public void PullJumpTrigger() => _animator.SetTrigger("Jump");
        public void PullAttackTrigger()
        {
            if (_isAttacking == false)
            {
                _animator.SetTrigger("Attack");
                _isAttacking = true;
            }
            else if (_canSetCombo == true)
            {
                _animator.SetBool("Combo", true);
                            Debug.Log("triggered");

                _canSetCombo = false;
            }
        }

        public void ResetAttack() => _isAttacking = false;
        public void ResetCombo() => _animator.SetBool("Combo", false);
        public void StartAcceptCombo() => _canSetCombo = true;
        public void StopAcceptCombo() => _canSetCombo = false;
        public void HitStarted() => _hitbox.SetActive(true);
        public void HitEnded() => _hitbox.SetActive(false);

        private void Update()
        {
            AnimatorSetBool();
        }
        private void AnimatorSetBool()
        {
            _animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetBool("Running", _isRunning);
            _animator.SetBool("Interacting", _isInteracting);
            _animator.SetFloat("YSpeed", _rigidbody.velocity.y);
        }
    }
}
