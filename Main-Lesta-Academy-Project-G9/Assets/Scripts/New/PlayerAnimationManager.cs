using System;
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
        [SerializeField] private MeshRenderer swordRenderer;
        [SerializeField] private float attackDelay = 0.5f;
        private bool showSword = false;
        private bool pressedAttack = false;
        private bool canAttack = true;
        private bool mustHide = false;
        Coroutine swordCoroutine = null;
        GameObject _projectile;

        private bool _isGrounded = true, _isRunning, _isInteracting, _combo, _isAttacking, _canSetCombo;
        private int _hitCounter = 0;


        public void SetGrounded(bool groundedToSet) => _animator.SetBool("IsGrounded", groundedToSet);


        public void SetRun(bool runToSet) => _isRunning = runToSet;
        public void SetInteracting(bool interacting) => _isInteracting = interacting;
        public void PullJumpTrigger() => _animator.SetTrigger("Jump");
        public void PullAttackTrigger()
        {
            if (canAttack)
            {
                _animator.SetTrigger("Attack");
                pressedAttack = true;
                StartCoroutine(CanAttack());
                if (!showSword)
                {
                    if (swordCoroutine != null)
                        StopCoroutine(swordCoroutine);
                    swordCoroutine = StartCoroutine(ShowSwordCoroutine());
                }
            }
        }

        public void PullSecondAttackTrigger()
        {
            if (canAttack && Mathf.Floor(_rigidbody.velocity.y) < 0.1f)
            {
                mustHide = true;
                _animator.SetTrigger("SecondAttack");
                StartCoroutine(CanAttackSuper());
            }
        }

        IEnumerator CanAttack()
        {
            canAttack = false;
            yield return new WaitForSeconds(attackDelay);
            canAttack = true;
        }

        IEnumerator CanAttackSuper()
        {
            canAttack = false;
            GetComponent<InputManager>().lockInput = true;
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 3; i++)
            {
                GameObject spawned = Instantiate(_projectile, this.transform.position + new Vector3(0f, 1.7f, 0f) + this.transform.forward, Quaternion.identity);
                CannonProjectile cp = spawned.GetComponent<CannonProjectile>();
                cp.throwType = CannonProjectile.type.LINE;
                cp.direction = this.transform.forward;
                cp.speed = 7.5f;
                cp.lifeTime = 10f;
                cp.damage = 15f;
                cp.enemiesProjectile = false;
                yield return new WaitForSeconds(0.45f);
            }
            yield return new WaitForSeconds(1f);
            canAttack = true;
            GetComponent<InputManager>().lockInput = false;
        }

        IEnumerator ShowSwordCoroutine()
        {
            showSword = true;
            float val = 2f;
            while (val > 1f)
            {
                val -= Time.deltaTime * 10f;
                swordRenderer.material.SetFloat("_Threshold", val);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            swordRenderer.material.SetFloat("_Threshold", 1f);
            swordRenderer.GetComponentInChildren<ParticleSystem>().Play();
            while (val > 0f)
            {
                val -= Time.deltaTime * 10f;
                swordRenderer.material.SetFloat("_Threshold", val);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            swordRenderer.material.SetFloat("_Threshold", 0f);
            int i = 0;
            while (i < 30 && !mustHide)
            {
                pressedAttack = false;
                yield return new WaitForSeconds(0.1f);
                i++;
                if (pressedAttack)
                    i = 0;
            }

            mustHide = false;

            showSword = false;
            while (val < 0.5f)
            {
                val += Time.deltaTime * 3f;
                swordRenderer.material.SetFloat("_Threshold", val);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            swordRenderer.material.SetFloat("_Threshold", 0.5f);
            swordRenderer.GetComponentInChildren<ParticleSystem>().Play();
            while (val < 3f)
            {
                val += Time.deltaTime * 3f;
                swordRenderer.material.SetFloat("_Threshold", val);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            swordRenderer.material.SetFloat("_Threshold", 3f);
            swordCoroutine = null;
        }

        public void ResetAttack() => _isAttacking = false;
        public void ResetCombo() => _animator.SetBool("Combo", false);
        public void StartAcceptCombo() => _canSetCombo = true;
        public void StopAcceptCombo() => _canSetCombo = false;
        public void HitStarted() => _hitbox.SetActive(true);
        public void HitEnded() => _hitbox.SetActive(false);

        public void AnimFirstAttack()
        {
            _animator.ResetTrigger("Attack");
            _isAttacking = true;
            _hitCounter++;
        }
        public void AnimSecondAttack()
        {
            _isAttacking = true;
            _hitCounter--;
        }
        public void Stop() { }//=> _isAttacking = false;

        private void Update()
        {
            AnimatorSetBool();
        }

        private void AnimatorSetBool()
        {
            _animator.SetBool("Interacting", _isInteracting);
            _animator.SetFloat("YSpeed", _rigidbody.velocity.y);
            _animator.SetFloat("XSpeed", UpdateAnimatorValue(Mathf.Abs(Input.GetAxis("Horizontal"))));
        }

        private float UpdateAnimatorValue(float horizontalSpeed)
        {
            if (horizontalSpeed > 0 && horizontalSpeed < 1) return 0.5f;
            else if (horizontalSpeed > -0.55f && horizontalSpeed < 0) return -0.5f;
            else if (horizontalSpeed > 0.55f) return 1f;
            else if (horizontalSpeed < -0.55f) return -1f;
            else return 0;
        }

        private void Start()
        {
            _projectile = Resources.Load<GameObject>("CannonProjectile");
        }

        public void PlayDeathAnimation()
        {
            _animator.Play("Death");
        }
    }
}
