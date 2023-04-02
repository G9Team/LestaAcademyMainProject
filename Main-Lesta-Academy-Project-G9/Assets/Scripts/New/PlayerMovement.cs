using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private GroundDetector _groundDetector;
        [SerializeField] private WallDetetor _wallDetector;
        [SerializeField] private LedgeDetector _ledgeDetector;
        private PlayerAnimationManager _animator;
        private List<IDetector> _detectors = new List<IDetector>();

        public int Direction { get; private set; }
        private IMover _mover;
        private float _moveKeeper;
        private bool _wallDetected, _grounded = true, _dubleJump = true, _canDash = true;
        private void Awake() {
            if (_groundDetector is not null) _detectors.Add(_groundDetector);
            if (_wallDetector is not null) _detectors.Add(_wallDetector);
           if (_ledgeDetector is not null) _detectors.Add(_ledgeDetector);

 
        }
        public void Initialize(PlayerInteractor interactor, IMover mover, PlayerAnimationManager manager)
        {
            _animator = manager;
            _detectors.Add(interactor);
            _mover = mover;


            foreach (IDetector detector in _detectors)
            {
                detector.OnDetectionApear += OnSomeDetection;
            }
        }
        private void OnSomeDetection(DetectionType typeOfDetection)
        {
            switch (typeOfDetection)
            {
                case DetectionType.Wall:
                    _wallDetected = !_wallDetected;
                    break;
                case DetectionType.Ground:
                    ProceedGroundDetection();
                    break;
                case DetectionType.HeavyInteraction:
                    ProceedHeavyInteraction();
                    break;
                case DetectionType.Ledge:
                    ProceedLedgeDetection();
                    break;
            }
        }
        private void ProceedGroundDetection()  {
            _grounded = _canDash = _dubleJump = true;
            _animator?.SetGrounded(_grounded);
        }

        private void ProceedLedgeDetection()
        {
            // Some ledge detection logic
        }

        private void ProceedHeavyInteraction()
        {
            // Some Heavy interactions
        }

        
        public void Move(float moveDirection) //insert params if necessary
        {
            float direct = _moveKeeper + moveDirection;
            if(_moveKeeper !=0 && Mathf.Abs(direct) < 1){
                _mover.Move(moveDirection);
                _moveKeeper = 0;

            }
            else if (_wallDetected) {
                _moveKeeper = _moveKeeper == 0 || moveDirection == 0? moveDirection :_moveKeeper;
                _mover.Move(0);
                _animator?.SetRun(false);
                return;
            }
            _mover.Move(moveDirection);

            if(moveDirection == 0) _animator?.SetRun(false);
            else _animator?.SetRun(true);


        }

        public void Jump()
        {
            if(_grounded)
            {
                RealJump(out _grounded);
            }
            else if (_dubleJump)
            {
               RealJump(out _dubleJump);
            }
        }
        private void RealJump(out bool couse){
            _mover.Jump();
            couse = false;
            _animator?.PullJumpTrigger();

        }

        public void Dash()
        {
            if(_canDash){
                _mover.Dash();
                _canDash = false;
            }
        }

        public void Attack(){
            _animator.PullAttackTrigger();
        }
        public void AnimFirst(){
            _animator.AnimFirstAttack();
            _mover.ChangeSpeed(0.5f);
        }
        public void AnimSecond(){
            _animator.AnimSecondAttack();
            _mover.ChangeSpeed(0.5f);

        }
        public void StopAnim(){
            _animator.Stop();
            _mover.ChangeSpeed(1f);
        }
        public void FirstAttack() => _mover.FirstAttack();
        public void SecondAttack() => _mover.SecondAttack();
        public void ThirdAttack() => _mover.ThirdAttack();
        public void StopAttack() => _mover.StopAttack();

        private void OnDisable()
        {
            foreach (IDetector detector in _detectors)
            {
                detector.OnDetectionApear -= OnSomeDetection;
            }

        }

    }
}
