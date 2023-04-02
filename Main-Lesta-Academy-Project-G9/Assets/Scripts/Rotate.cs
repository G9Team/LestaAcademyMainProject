using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 dir;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(dir, speed * Time.deltaTime);
    }
}
