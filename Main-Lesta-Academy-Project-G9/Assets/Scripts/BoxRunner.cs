using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxRunner : MonoBehaviour
{
    private Rigidbody _rigidBody;
    public float RunSpeed;
    [SerializeField] private Transform _forward;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _everythingButPlayerAndEnemy;
    private bool _isFacingRight;
    public event UnityAction OnDirectionChange;
    public float Direction {get=> _direction;
     private set{
        if (_direction != value) {
        OnDirectionChange?.Invoke();
        _direction = value;
        }
     }}
    private float _direction;
    private float _move;
    private JumperTest _jumpController;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _jumpController = GetComponent<JumperTest>();
    }   
    private void Update()
    {
        _move = Input.GetAxis("Horizontal");
        if(_move != 0) Direction = Mathf.Sign(_move);
    }
    private void FixedUpdate()
    {
        if (_jumpController.IsDashing || _move == 0) 
        {
            _animator.SetBool("run", false);
            return;
        }
        if(!_animator.GetBool("run")){
            _animator.SetBool("run", true);
        }
        _rigidBody.velocity = new Vector3(_move * RunSpeed, _rigidBody.velocity.y, 0);
        if (IsForwardWallColidied())
        {
            _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, 0);
        }
        if (_move > 0 && !_isFacingRight) Flip(_move);
        else if (_move < 0 && _isFacingRight) Flip(_move);
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
    private bool IsForwardWallColidied()
    {
        for (int i = 0; i < _forward.childCount; i++)
        {
            if (Physics.CheckSphere(_forward.GetChild(i).position, 0.05f, _everythingButPlayerAndEnemy))
            {
                return true;
            }
        }
        return false;
    }
}
