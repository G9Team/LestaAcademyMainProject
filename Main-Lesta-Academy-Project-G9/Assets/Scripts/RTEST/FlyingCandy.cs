using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCandy : MonoBehaviour
{

    private Animator _animator;
    private Rigidbody _candyRigidbody;
    public float speed = 500f;


    private int _direction = 1;
    private int _startX;
    private int _finishX;
    private int maxDistance = 10;

 

    // Start is called before the first frame update
    void Start()
    {
        _candyRigidbody = GetComponent<Rigidbody>();
        _startX = (int)_candyRigidbody.position.x;
        _finishX = _startX + maxDistance;
   
    }

    // Update is called once per frame
    void Update()
    {
        if (_candyRigidbody != null) {

            _candyRigidbody.velocity = new Vector3(speed * _direction * Time.deltaTime, 0, 0);

            
            if (_finishX - (int)_candyRigidbody.position.x == 0){
               // Debug.Log("flyyy" + _candyRigidbody.position.x);

                int buf = _startX;
                _startX = _finishX;
                _finishX = buf;
                _direction *= -1;

             
               transform.Rotate(new Vector3(0, 180, 0), Space.Self);

            }
        }
    }





}
