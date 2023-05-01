using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace New
{
    public class BulletToHell : MonoBehaviour
    {
        [SerializeField] private float _flyDuration, _fireDelay;
        private Vector3 _destination1, _destination2;
        private void Awake() {
            _destination1 = (this.transform.localPosition - this.transform.parent.localPosition) * -0.5f;
            _destination2 = (this.transform.position - this.transform.parent.position) * 20;
        }
        public void Fire(){
            this.transform.DOMove(_destination2, _flyDuration)
                .SetEase(Ease.Linear)
                .SetDelay(_fireDelay)
                .OnComplete(() => Destroy(this.gameObject));
            this.transform.SetParent(null); 

        }

        //TODO little bounce, if i can calculate right position
        private void ActualFire(){
            this.transform.DOMove(_destination2, _flyDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(this.gameObject));
        }


    }

}
