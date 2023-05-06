using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : MonoBehaviour {
    private Vector3 _target;
    public Vector3 direction;
    public float speed = 5f;
    public float lifeTime = 10f;

    public enum type
    {
        LINE = 1,
        CURVE = 2
    }

    public type throwType = type.LINE;
    
    float _startTime;
    Vector3 _startPos;
    float _height = 10f;
    
    
    private void Start() {
        if (throwType == type.CURVE)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform.position;
            _startTime = Time.time;
            _startPos = this.transform.position;
        }
        Invoke("DestroyThis", lifeTime);
    }

    void DestroyThis()
    {
        Destroy(this.gameObject);
    }
    
    void Update()
    {
        if (throwType == type.CURVE)
        {
            float time = (Time.time - _startTime) * speed / 10f;
            float target_X = _startPos.x + (_target.x - _startPos.x) * time;
            float target_Y = _startPos.y + ((_target.y - _startPos.y)) * time + _height * (1-(Mathf.Abs(0.5f - time) / 0.5f) * (Mathf.Abs(0.5f - time) / 0.5f));
            this.transform.position = new Vector3(target_X, target_Y);
        }
        else
        {
            this.transform.position += direction * speed * Time.deltaTime;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        IAlive alive = other.GetComponent<IAlive>();
        if(alive != null)
            alive.ChangeHealth(-1);
    }
}
