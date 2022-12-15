using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpHeight;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private bool isFacingRight;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isWallDetected;
    private bool isWallSliding;
    private bool canWallSlide;
    private bool canWallJump = true;
    private bool canMove = true;

    private int facingDirection = 1;
    [SerializeField] private Vector3 wallJumpDirection;

    public LayerMask groundLayer;
    public Transform groundChecker;

    private float _groundCheckRadius = 0.4f;
    private Collider[] _groundCollisions;

    public Transform wallChecker;
    public float wallCheckDistance;
    public LayerMask wallLayer;      


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canMove = true;
            Jump();
        }

        CheckCollision();
        AnimatorController();
        FlipController();
    }

    void FixedUpdate()
    {
       
        if (Input.GetAxis("Vertical") < 0)
        {
            canWallSlide = false;
        }

        if (isWallDetected && canWallSlide)
        {
            isWallSliding = true;
            playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y * 0.1f);
            canMove = false;
        }

        else if (!isWallDetected) 
        { 
            isWallSliding = false; 
            Move(); 
        }
    }

    private void Jump()
    {

        canWallSlide = false;
        if (isWallSliding && canWallJump)
        {
            WallJump();
        }
        else if (isGrounded)
        {
            isWallSliding = false;
            playerRigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
            canDoubleJump = true;
        }
            
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            playerRigidbody.velocity += Vector3.up * _jumpHeight * 0.08f;        
        }      
    }

    private void WallJump()
    {
        Vector3 direction = new Vector3(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);     
        playerRigidbody.AddForce(direction * _jumpHeight * 80 * Time.deltaTime,ForceMode.Impulse);
        Flip();
        canDoubleJump = true;
    }

    private void FlipController()
    {
        float move = Input.GetAxis("Horizontal");
        if (isGrounded && isWallDetected)
        {
            if (isFacingRight && move < 0)
            {
                Flip();
            }
            else if (!isFacingRight && move > 0) Flip();
        }
    }

    private void Move()
    {
        if (canMove)
        {
            float move = Input.GetAxis("Horizontal");
            UpdateAnimatorValue(move);
            playerRigidbody.velocity = new Vector3(move * _runSpeed, playerRigidbody.velocity.y, 0);

            if (move > 0 && isFacingRight) Flip();
            else if (move < 0 && !isFacingRight) Flip();
        }
        
    }

    private void Flip() 
    {
        facingDirection = -facingDirection;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        wallCheckDistance = -wallCheckDistance;
    }

    private void CheckCollision()
    {
        _groundCollisions = Physics.OverlapSphere(groundChecker.position, _groundCheckRadius, groundLayer);
        if (_groundCollisions.Length > 0) { isGrounded = true; canMove = true; }
        else isGrounded = false;

        isWallDetected = Physics.Raycast(wallChecker.position, Vector2.right, wallCheckDistance, wallLayer);
        if(isWallDetected) Debug.Log("detected");
        
        if (!isGrounded && playerRigidbody.velocity.y < 0) canWallSlide = true;
    }

    private void AnimatorController()
    {
        playerAnimator.SetBool("IsGrounded", isGrounded);
        playerAnimator.SetBool("IsWallSliding", isWallSliding);
    }

    private void UpdateAnimatorValue(float horizontalSpeed)
    {
        float h = 0;
        if (horizontalSpeed > 0 && horizontalSpeed < 1)  h = 0.5f;
        else if (horizontalSpeed > -0.55f && horizontalSpeed < 0)  h = -0.5f;
        else if (horizontalSpeed > 0.55f)  h = 1;
        else if (horizontalSpeed < -0.55f)  h = -1;

        playerAnimator.SetFloat("Speed", Mathf.Abs(h));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallChecker.position, new Vector3(wallChecker.position.x + wallCheckDistance, wallChecker.position.y, wallChecker.position.z));
    }
}
