using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField] public float runSpeed;
    [SerializeField] private float _jumpforce;
    [SerializeField] private GameObject _hitboxPrefab;
    [SerializeField] private Transform _legsPosition;
    Rigidbody playerRigidbody;
    Animator playerAnimator;
    private int _jumpcounter, _maxJumps = 1;
    bool isFacingRight, _isAtacking = false, _isInvincible = false;
    private int _health = 4;
    public int AtackForce {get; private set; } = 1;
    void Start()
    {
        _jumpcounter = _maxJumps;
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if((Input.GetKey(KeyCode.E) || Input.GetMouseButton(0)) && !_isAtacking){
            Atack();
        }
        CheckForJumpingConditions();
        float move = Input.GetAxis("Horizontal");
        playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        playerRigidbody.velocity = new Vector3(move * runSpeed, playerRigidbody.velocity.y, 0);
        if(move>0 && isFacingRight) Flip();
        else if(move<0 && !isFacingRight) Flip();
        
    }
    private void CheckForJumpingConditions(){
        playerRigidbody.AddForce(Physics.gravity * 5 * playerRigidbody.mass);
        if (Physics.CheckSphere(_legsPosition.position, 0.05f)) _jumpcounter = _maxJumps;
        if (Input.GetKey(KeyCode.Space) && _jumpcounter > 0) {
        playerRigidbody.AddForce(Vector3.up * _jumpforce, ForceMode.Impulse);
        _jumpcounter--;
        }
    }
    void Flip() 
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.z = -localScale.z;
        transform.localScale = localScale;
    }
    public void GetDamage(int acceptedDamage) => StartCoroutine(ApplyDamage(acceptedDamage));
    private IEnumerator ApplyDamage(int acceptedDamage){
        if (_isInvincible) yield return null;
        if(_health - acceptedDamage <= 0) Die();
        Debug.Log("hit! ");
        _isInvincible = true;
        _health -= acceptedDamage;
        Debug.Log("Damage accepted " + acceptedDamage + " Health left: " + _health);
        yield return new WaitForSeconds (0.6f);
        _isInvincible = false;

    }
    private void Die(){
        Debug.Log("U are dead ");
        Destroy(this.gameObject);
    }
    private void Atack(){
        StartCoroutine(HitBoxApplier());
    }
    private IEnumerator HitBoxApplier(){
        _isAtacking = true;
        GameObject hitbox = Instantiate(_hitboxPrefab, 
            new Vector3(
                this.transform.position.x + (isFacingRight ? -0.5f : 0.5f), 
                this.transform.position.y + 1.5f, 
                this.transform.position.z),
            Quaternion.identity, this.transform);
        Destroy(hitbox, 0.2f);
        yield return new WaitForSeconds(0.4f);
        _isAtacking = false;
    }
}
