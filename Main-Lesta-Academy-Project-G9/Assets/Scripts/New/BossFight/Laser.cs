using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{

    public class Laser : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line, _actualLaser;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private float _laserDelay, _laserDuration;

        private GameObject _lineInstance, _laserInstance;
        private Vector3 _rayDirection;
        private bool _casting = false;
        private Transform _initialPlayerPos;
        
        private void Awake() {
            _initialPlayerPos = GameObject.FindGameObjectWithTag("Player").transform;

        }
        public void FireAtPlayer(){
            Vector3[] laserPoints = new Vector3[]{
                this.transform.position,  
                _initialPlayerPos.position, 
                new Vector3(
                    _initialPlayerPos.position.x + (_initialPlayerPos.position.x - this.transform.position.x) * 2, 
                    _initialPlayerPos.position.y + (_initialPlayerPos.position.y - this.transform.position.y) * 2
                )
            };
            ActualFire(laserPoints);
        }

        public void FireParalell(){
            Vector3 laserTo = new Vector3(
                (this.transform.position.x + 2) * -10,
                this.transform.position.y
            );
            Vector3[] laserPoints = new Vector3[]{
                this.transform.position,
                laserTo,
                laserTo
            };
            ActualFire(laserPoints);
        }

        private void ActualFire(Vector3[] laserPoints){
            _rayDirection = (laserPoints[1] - laserPoints[0]);
            _line.SetPositions(laserPoints);
            _actualLaser.SetPositions(laserPoints);

            _lineInstance = Instantiate(_line).gameObject;
            StartCoroutine(StartLaserAttack());
        }
        private void OnDrawGizmos() {
            Gizmos.DrawLine(this.transform.position, _rayDirection);
        }
        private void FixedUpdate() {
            
            if (_casting){
                RaycastHit hit = new RaycastHit();   
                if (Physics.SphereCast(
                    this.transform.position, 
                    0.5f, 
                    _rayDirection, 
                    out hit, 
                    40f,
                    _playerLayer
                ))
                {
                    hit.transform.gameObject.GetComponent<PlayerComponentManager>().DamagePlayer(-1);
                }
            }
            

        }

        private IEnumerator StartLaserAttack(){
            yield return new WaitForSeconds(_laserDelay);
            _casting = true;
            _laserInstance = Instantiate(_actualLaser, _lineInstance.transform.position, Quaternion.identity).gameObject;
            yield return new WaitForSeconds(_laserDuration);
            _casting = false;
            this.gameObject.SetActive(false);
        }

        private void OnDisable() {
            Destroy(_lineInstance);
            Destroy(_laserInstance, 0.05f);
        }
    }
}
