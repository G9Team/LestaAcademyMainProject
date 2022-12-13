using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpHeight;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private bool isFacingRight;
    private bool isGrounded;
    private bool isAbleDoubleJump;

    public LayerMask groundLayer;
    public Transform groundChecker;

    private float _groundCheckRadius = 0.3f;
    private Collider[] _groundCollisions;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            playerAnimator.SetBool("IsGrounded", false);
            playerRigidbody.AddForce(new Vector3(0, _jumpHeight, 0), ForceMode.Impulse);
            isAbleDoubleJump = true;
        }

        else if(!isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (isAbleDoubleJump)
            {
                playerRigidbody.velocity = Vector3.up * _jumpHeight * 0.08f;
                isAbleDoubleJump = false;
            }
        }

        _groundCollisions = Physics.OverlapSphere(groundChecker.position, _groundCheckRadius, groundLayer);
        if (_groundCollisions.Length > 0) isGrounded = true;
        else isGrounded = false;

        playerAnimator.SetBool("IsGrounded", isGrounded);

    }

    void FixedUpdate()
    {
        Move(); 
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");
        UpdateAnimatorValue(move);
        playerRigidbody.velocity = new Vector3(move * _runSpeed, playerRigidbody.velocity.y, 0);
        if (move > 0 && isFacingRight) Flip();
        else if (move < 0 && !isFacingRight) Flip();
    }

    private void Flip() 
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.z = -localScale.z;
        transform.localScale = localScale;
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
}
