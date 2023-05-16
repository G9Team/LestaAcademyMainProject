using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    GameObject _projectile;
    public float speed;
    public float lifeTime;
    void Start()
    {
        _projectile = Resources.Load<GameObject>("CannonProjectile");
        InvokeRepeating("SpawnProjectile", 1f, 1f);
    }

    void SpawnProjectile()
    {
        GameObject spawned = Instantiate(_projectile, this.transform.position, Quaternion.identity);
        CannonProjectile cp = spawned.GetComponent<CannonProjectile>();
        cp.throwType = CannonProjectile.type.LINE;
        cp.direction = this.transform.forward;
        cp.speed = speed;
        cp.lifeTime = lifeTime;
    }
}
