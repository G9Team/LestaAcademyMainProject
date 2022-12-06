using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float runSpeed;

    Rigidbody playerRigidbody;
    Animator playerAnimator;

    bool isFacingRight;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
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
