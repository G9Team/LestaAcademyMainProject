using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float runSpeed;
    [SerializeField] private float _jumpforce;
    [SerializeField] private Transform _legsPoint;
    Rigidbody playerRigidbody;
    Animator playerAnimator;
    private int _jumpcounter, _maxJumps = 1;
    bool isFacingRight;

    void Start()
    {
        _jumpcounter = _maxJumps;
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        playerRigidbody.AddForce(Physics.gravity * 5 * playerRigidbody.mass);
        if (Physics.CheckSphere(transform.position, 0.1f)) _jumpcounter = _maxJumps;
        if (Input.GetKey(KeyCode.Space) && _jumpcounter > 0) {
        playerRigidbody.AddForce(Vector3.up * _jumpforce, ForceMode.Impulse);
        
        _jumpcounter--;
        }
        float move = Input.GetAxis("Horizontal");
        playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        playerRigidbody.velocity = new Vector3(move * runSpeed, playerRigidbody.velocity.y, 0);
        if(move>0 && isFacingRight) Flip();
        else if(move<0 && !isFacingRight) Flip();
        
    }

    void Flip() 
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.z = -localScale.z;
        transform.localScale = localScale;
    }
}
