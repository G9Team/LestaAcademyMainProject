using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _runSpeed;
    private GameObject _player = null;
    private int _health = 3;
    private int _atackDamage = 1;
    private Rigidbody _rigidBody;
    private bool _isInvincible = false, _isDead = false;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (_isDead || _isInvincible) return;   
        if (Vector3.Distance(this.transform.position, _player.transform.position) < 10f && !_isInvincible)
        {
            FollowPlayer(_player.transform.position);
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PlayerAtackHitbox" && !_isInvincible) {
            StartCoroutine(Invulnerability());
        var battlePlayer = other.transform.parent.GetComponent<PlayerBattleData>();
        GetDamage(battlePlayer.AtackForce);
        GetKnockback(battlePlayer.KnockbackForce);
        }
        

    }
    private IEnumerator Invulnerability(){
        _isInvincible = true;
        yield return new WaitForSeconds(0.15f);
        _isInvincible = false;
    }
   
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.collider);
        PlayerBattleData player = other.gameObject.GetComponent<PlayerBattleData>();
        //Debug.Log(player);
        if (player is null) return;
        player.GetDamage(_atackDamage);
    }
    private void FollowPlayer(Vector3 playerPosition)
    {
        //Debug.Log("Following Player");
        float direction = _runSpeed * Mathf.Sign(playerPosition.x - transform.position.x);
        //Debug.Log(Mathf.Abs(playerPosition.x - transform.position.x));
        _rigidBody.velocity = new Vector3(direction, _rigidBody.velocity.y, 0);
        //transform.Translate((playerPosition - transform.position) * _runSpeed);
    }
    private void GetDamage(int inputDamage)
    {
        if (_health - inputDamage <= 0) Die();
        _health -= inputDamage;
        Debug.Log("Auch, it hurts!");
    }
    private void Die()
    {
        _isDead = true;
        Debug.Log("OMG, u killed Teddy bear!!! He is freaking DEAD now! ");
        _animator.SetBool("die", true);
        Destroy(this.gameObject, 1f);
    }
    public void GetKnockback(Vector3 knockbackForce){
        _rigidBody.AddForce(knockbackForce, ForceMode.Impulse);
        print("Knockback: " + knockbackForce);
    }
}
