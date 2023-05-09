using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace New
{
    public class FireBall : MonoBehaviour
    {
        [SerializeField] private float _fireballFlyDuration, _fireDelay, _v2FlySpeed, _v2StepDuration;
        [SerializeField]private int _stepCounter = 3;
        private Transform _player;
        private Vector3 _curentDirection;
        private float _stepDuration;
        
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _curentDirection =  _player.position - this.transform.position;
            _stepDuration = _v2StepDuration;
            this.transform.SetParent(null);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            /*
            Vector3[] path = GetWaypoints();
            

            this.transform.DOPath(path, _fireballFlyDuration, PathType.CatmullRom, PathMode.Full3D, 10, Color.red)
                .SetDelay(_fireDelay)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(this.gameObject))
                .OnStart(() => this.transform.SetParent(null));
                */

        }

        private void Update() {
            if(_fireDelay > 0){
                _fireDelay -= Time.deltaTime;
                return;
            }
            if(_stepDuration < 0){
                Debug.Log(_curentDirection + " " + _player.position);
                _stepDuration = _v2StepDuration;
                _curentDirection = _player.position - this.transform.position;
                _stepCounter--;
            }
            if (_stepCounter <= 0){
                Destroy(this.gameObject);
            }
            this.transform.Translate(_curentDirection * Time.deltaTime * _v2FlySpeed);
            _stepDuration -= Time.deltaTime;
        }

        private Vector3[] GetWaypoints(){
            Vector3[] path = new Vector3[2];
            path[1] = _player.position;
            Vector3 waypoint = GetSecondWaypoint(_player.position, this.transform.position - (this.transform.position - _player.position)/2);
            path[0] = waypoint;
            return path;
        }

        private Vector3 GetSecondWaypoint(Vector3 playerPosition, Vector3 midlePosition){
            float overalStep = Vector3.Distance(playerPosition, this.transform.position) * 0.2f;
            float sideStep = Mathf.Sqrt((overalStep * overalStep) / 2);
            float sign = Mathf.Sign(playerPosition.x - this.transform.position.x);

            return new Vector3(
                midlePosition.x + (sideStep * sign),
                midlePosition.y + sideStep,
                0
            );
        }
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player"){
                Destroy(this.gameObject);
            }
        }

    }

}
