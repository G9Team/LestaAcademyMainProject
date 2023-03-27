using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{
    public class InputManager : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerInteractor _interactor;
        private float _move, _exMove;
        private bool _moveflag;

        public void Initialize(PlayerInteractor interactor, PlayerMovement movement)
        {
            _interactor = interactor;
            _playerMovement = movement;
        }

        void Update()
        {
            _playerMovement.Move(Input.GetAxis("Horizontal"));

            if (Input.GetKeyDown(KeyCode.E))
            {
                _interactor.OnInteractionInput();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerMovement.Jump();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)){
                _playerMovement.Dash();
            }
            if (Input.GetKeyDown(KeyCode.K)){
                _playerMovement.Attack();
            }
        }
        private void ProceedMove()
        {
            /*
            if (_exMove == 0)
            {
                _playerMovement.Move(_move);
                return;
            }
            else if (Mathf.Abs(_exMove) > Mathf.Abs(_move))
            {
                _playerMovement.Move(0);
                return;
            }
            else
            {
                _playerMovement.Move(Mathf.Sign(_move));
            }
            */
            _playerMovement.Move(_move);
            StopAllCoroutines();
            StartCoroutine(MoveStoper());
        }
        private IEnumerator MoveStoper(){
            yield return new WaitForSeconds(0.1f);
            _moveflag = false;

        }
    }
}
