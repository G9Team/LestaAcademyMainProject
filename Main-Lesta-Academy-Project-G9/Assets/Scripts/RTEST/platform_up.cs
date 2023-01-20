using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_up : MonoBehaviour
{
    private float _speed = 5.0f;
    private float _direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
        transform.Translate(0, _speed * Time.deltaTime * _direction, 0);

        if (transform.position.y >= 41 || transform.position.y <= 24.95) {
            _direction *= -1;
        }

  
    }
}
