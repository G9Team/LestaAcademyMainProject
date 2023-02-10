using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackAnimationController : MonoBehaviour
{
    [SerializeField] private float _firstAttackSpeedX, _secondAttackSpeedX, _thirdAttackSpeedX;
    private float _currentAttackSpeedX;
    private Rigidbody _rigidBody;
    public bool IsAttacking {get; private set; } = false;
    private BoxRunner _runner;
    private void Awake() {
        _rigidBody = GetComponent<Rigidbody>();
        _runner = GetComponent<BoxRunner>();
    }

    void FixedUpdate()
    {
        
        if (IsAttacking == false){
            return;
        } 

        
        _rigidBody.velocity = new Vector3(_currentAttackSpeedX, 0, 0);
    }
    public void FirstAttack(){
        _currentAttackSpeedX = _firstAttackSpeedX * _runner.Direction;
        IsAttacking = true;
    }
    public void SecondAttack(){
        _currentAttackSpeedX = _secondAttackSpeedX * _runner.Direction;
        IsAttacking = true;
    }
    public void ThirdAttack(){
        _currentAttackSpeedX = _thirdAttackSpeedX * _runner.Direction;
        IsAttacking = true;
    }
    public void StopAttack() => IsAttacking = false;
}
