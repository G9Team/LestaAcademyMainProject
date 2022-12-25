using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _mainGravityScaler;
    [SerializeField] private float _fallGravityScaler;

    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;

    private bool canDoubleJump;
    private bool canWallJump = true;
    private bool canMove = true;
    private bool isWallDetected;
    private bool isWallSliding;
    private bool isFacingRight;
    private bool canWallSlide;
    private int facingDirection = 1;
    
    [Header("Ground Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundChecker;
    private float _groundCheckRadius = 0.4f;
    private Collider[] _groundCollisions;
    private bool isGrounded;

    [Header("Wall Settings")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallChecker;
    private float _wallCheckRadius = 0.55f;
    private Collider[] _wallCollisions;

    private Vector2 offset1;
    private Vector2 offset2;

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrab = true;
    private bool canClimb;

    [HideInInspector] public bool ledgeDetected;

    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckCollision();
        CheckLedge();
        AnimatorController();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            canMove = true;
            JumpController();
        }
    }

    void FixedUpdate()
    {
        if (isWallDetected && canWallSlide)
        {
            isWallSliding = true;
            _playerRigidbody.velocity = new Vector3(0, _playerRigidbody.velocity.y *0.8f);
            canMove = false;
        }

        else if (!isWallDetected) 
        { 
            isWallSliding = false; 
            Move(); 
        }

        GravityHandler();
        _playerRigidbody.velocity += Vector3.up * _playerRigidbody.mass * _mainGravityScaler * -1;
    }

    private void JumpController()
    {
        canWallSlide = false;
        if (isWallSliding && canWallJump)
        {
            WallJump();
        }
        else if (isGrounded)
        {
            isWallSliding = false;
            Jump();
            canDoubleJump = true;    
        }       
        else if (canDoubleJump)
        {
            Jump();  
            canDoubleJump = false;       
        }      
    }

    private void CheckLedge()
    {
        if (ledgeDetected && canGrab)
        {
            canGrab = false;
            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        }
            if (canClimb)
                transform.position = climbBegunPosition;    
        
    }

    private void Jump()
    {
        float tempVelocity = _playerRigidbody.velocity.x;
        _playerRigidbody.velocity = Vector3.zero;
        _playerRigidbody.velocity += Vector3.up * _jumpHeight;
        _playerRigidbody.velocity += new Vector3(tempVelocity, 0, 0);
    }

    private void GravityHandler()
    {
        if (_playerRigidbody.velocity.y < 0)
        {
            _playerRigidbody.velocity += Vector3.up * _fallGravityScaler;
        }
    }

    private void WallJump()
    {
        isWallDetected = false;
        canWallSlide = false;
        float tempVelocity = _playerRigidbody.velocity.x;
        _playerRigidbody.velocity = Vector3.zero;
        _playerRigidbody.velocity += Vector3.up * _jumpHeight;
        _playerRigidbody.velocity += new Vector3(tempVelocity, 0, 0);
    }

    private void Move()
    {
        if (canMove)
        {
            float move = Input.GetAxis("Horizontal");
            UpdateAnimatorValue(move);
            _playerRigidbody.velocity = new Vector3(move * _runSpeed, _playerRigidbody.velocity.y, 0);
          

            if (move > 0 && isFacingRight) Flip();
            else if (move < 0 && !isFacingRight) Flip();
        }       
    }

    private void Flip() 
    {
        facingDirection = -facingDirection;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CheckCollision()
    {
        _groundCollisions = Physics.OverlapSphere(groundChecker.position, _groundCheckRadius, groundLayer);
        if (_groundCollisions.Length > 0) { isGrounded = true; canMove = true; }
        else isGrounded = false;
        
        _wallCollisions = Physics.OverlapSphere(wallChecker.position, _wallCheckRadius, wallLayer);
        if (_wallCollisions.Length > 0) { isWallDetected = true;  }
        else { isWallDetected = false; }

        if (!isGrounded && _playerRigidbody.velocity.y < 0) canWallSlide = true;

    }

    private void AnimatorController()
    {
        _playerAnimator.SetBool("IsGrounded", isGrounded);
        _playerAnimator.SetBool("IsWallSliding", isWallSliding);
    }

    private void UpdateAnimatorValue(float horizontalSpeed)
    {
        float h = 0;
        if (horizontalSpeed > 0 && horizontalSpeed < 1)  h = 0.5f;
        else if (horizontalSpeed > -0.55f && horizontalSpeed < 0)  h = -0.5f;
        else if (horizontalSpeed > 0.55f)  h = 1;
        else if (horizontalSpeed < -0.55f)  h = -1;

        _playerAnimator.SetFloat("Speed", Mathf.Abs(h));
    }

}
