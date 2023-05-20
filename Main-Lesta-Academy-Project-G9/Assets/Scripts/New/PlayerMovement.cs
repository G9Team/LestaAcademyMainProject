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
        private FootstepSound _audio;
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
        public void Initialize(PlayerInteractor interactor, IMover mover, PlayerAnimationManager manager, FootstepSound audio)
        {
            _animator = manager;
            _detectors.Add(interactor);
            _mover = mover;
            _audio = audio;


            foreach (IDetector detector in _detectors)
            {
                detector.OnDetectionApear += OnSomeDetection;
            }
        }
        private void OnSomeDetection(DetectionType typeOfDetection, bool grounded)
        {
            switch (typeOfDetection)
            {
                case DetectionType.Wall:
                    _wallDetected = !_wallDetected;
                    break;
                case DetectionType.Ground:
                    ProceedGroundDetection(grounded);
                    break;
                case DetectionType.HeavyInteraction:
                    ProceedHeavyInteraction();
                    break;
                case DetectionType.Ledge:
                    ProceedLedgeDetection();
                    break;
            }
        }
        private void ProceedGroundDetection(bool grounded)  {
            if(grounded){
               if(!_grounded){
                    _audio.PlayCustom("grounding");
               }
               _canDash = _dubleJump = grounded;
            }
            _grounded = grounded;
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
               
                return;
            }
            _mover.Move(moveDirection); 

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
            _animator?.SetGrounded(_grounded);
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
        
        public void SecondAttack(){
            _animator.PullSecondAttackTrigger();
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
