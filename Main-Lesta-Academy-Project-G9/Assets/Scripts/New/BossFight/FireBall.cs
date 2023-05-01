using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace New
{
    public class FireBall : MonoBehaviour
    {
        [SerializeField] private float _fireballFlyDuration, _fireDelay;
        void Start()
        {
            Vector3[] path = GetWaypoints();

            this.transform.DOPath(path, _fireballFlyDuration, PathType.CatmullRom, PathMode.Full3D, 10, Color.red)
                .SetDelay(_fireDelay)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(this.gameObject))
                .OnStart(() => this.transform.SetParent(null));

        }
        private Vector3[] GetWaypoints(){
            Vector3[] path = new Vector3[2];
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            path[1] = player.position;
            Vector3 waypoint = GetSecondWaypoint(player.position, this.transform.position - (this.transform.position - player.position)/2);
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

    }

}
