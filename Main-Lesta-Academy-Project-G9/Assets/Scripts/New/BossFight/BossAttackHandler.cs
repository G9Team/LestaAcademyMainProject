using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    public class BossAttackHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _spike, _fireball, _bulletToHell, _bossBody;
        [SerializeField] private Laser _laser;
        [SerializeField] private Transform _fireFrom;
        [SerializeField] private float _bulletHellRadius, _bulletHellAppearanceDelay, _maxBulletHellOffset;
        [SerializeField] private int _bulletsCount;
        [SerializeField] private Animator _animator;


        private void Update() {
            if(Input.GetKeyDown(KeyCode.X)){
                BulletHell();
            }
            if(Input.GetKeyDown(KeyCode.C)){
                RailGun();
            }
            if(Input.GetKeyDown(KeyCode.Z)){
                FloorIsLava();
            }
        }
        public void ProceedAttack(BossAttackType type)
        {
            switch (type)
            {
                case BossAttackType.FireBall:
                    FireBall();
                    break;

                case BossAttackType.Spikes:
                    Spikes();
                    break;

                case BossAttackType.RailGun:
                    RailGun();
                    break;

                case BossAttackType.FloorIsLava:
                    FloorIsLava();
                    break;

                case BossAttackType.BulletHell:
                    BulletHell();
                    break;
            }
        }

        private void FireBall()
        {
            _animator.Play("Attack1");

            Instantiate(_fireball, _fireFrom, false);
        }
        private void Spikes()
        {
            _animator.Play("Attack3");

            Instantiate(_spike, new Vector3(0, -2, 0), Quaternion.identity);
        }
        private void RailGun()
        {
            _animator.Play("Attack2");

            _laser.gameObject.SetActive(true);
            _laser.FireAtPlayer();
        }
        private void FloorIsLava()
        {
            _animator.Play("Attack2");
            _laser.gameObject.SetActive(true);
            _laser.FireParalell();

        }
        private void BulletHell()
        {
            _animator.Play("Attack1");

           StartCoroutine(SpawnBulletsToHell(Random.Range(0, _maxBulletHellOffset)));

        }
        
        private IEnumerator SpawnBulletsToHell(float offset){
            List<BulletToHell> bullets = new List<BulletToHell>();
            for (int i = 0; i < _bulletsCount; i++)
            {
                float angle = i * Mathf.PI * 2f / _bulletsCount + offset;
                Vector3 newPos = _fireFrom.position + new Vector3(Mathf.Cos(angle) * _bulletHellRadius, Mathf.Sin(angle) * _bulletHellRadius);
                BulletToHell bullet = Instantiate(_bulletToHell, newPos, Quaternion.identity, _fireFrom).GetComponent<BulletToHell>(); 
                bullets.Add(bullet);                
                yield return new WaitForSeconds(_bulletHellAppearanceDelay);
            }
            foreach(var bullet in bullets){
                bullet.Fire();
            }
        }
    }

}
