using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{

    public class PlayerComponentManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private VelocityHandler _velocityHandler;
        [SerializeField] private PlayerAnimationManager _animationManager;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PlayerInteractionsChecker _interactionChecker;
        private IPlayerData _playerData = new PlayerData();
        private IUpgrader _upgrader;
        private PlayerInteractor _interactor;
        private void Awake() {
            _upgrader = new PlayerUpgrader(_playerData);
            _interactor = new PlayerInteractor(_upgrader, _interactionChecker);

            _playerMovement.Initialize(_interactor, _velocityHandler, _animationManager);
            _inputManager.Initialize(_interactor, _playerMovement);
        }
        public IPlayerData GetPlayerData(){
            return this._playerData;
        }
        public VelocityHandler GetVelocityHandler(){
            return this._velocityHandler;
        }
    }
}
