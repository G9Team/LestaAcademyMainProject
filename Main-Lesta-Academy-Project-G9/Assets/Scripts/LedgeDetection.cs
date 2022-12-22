using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private PlayerController _player;
    [SerializeField] private LayerMask _groundLayer;
    private Collider[] _groundCollisions;
    private bool canDetected;


    private void Update()
    {
        if (canDetected)
        {
            _groundCollisions = Physics.OverlapSphere(transform.position, _radius, _groundLayer);
            if (_groundCollisions.Length > 0) _player.ledgeDetected = true;
            else _player.ledgeDetected = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
