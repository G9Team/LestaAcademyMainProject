using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _runSpeed;
    private GameObject _player = null;
    private int _health = 2;
    private int _atackDamage = 1;
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        
        if (Vector3.Distance(this.transform.position, _player.transform.position) < 10f)
        {
            FollowPlayer(_player.transform.position);
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PlayerAtackHitbox") {
        int damage = other.transform.parent.GetComponent<NewPlayerController>().AtackForce;
        GetDamage(damage);
        }
        

    }
   
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.collider);
        NewPlayerController player = other.gameObject.GetComponent<NewPlayerController>();
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
        Debug.Log("OMG, u killed Teddy bear!!! He is freaking DEAD now! ");
        Destroy(this.gameObject);
    }
}
