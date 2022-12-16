using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRunner : MonoBehaviour
{
    private Rigidbody _rigidBody;
    public float RunSpeed;
    [SerializeField] private Transform _forward;
    private bool _isFacingRight;
    private float _move;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _move = Input.GetAxis("Horizontal");

    }
    private void FixedUpdate()
    {
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
            if (Physics.CheckSphere(_forward.GetChild(i).position, 0.05f))
            {
                return true;
            }
        }
        return false;
    }
}
