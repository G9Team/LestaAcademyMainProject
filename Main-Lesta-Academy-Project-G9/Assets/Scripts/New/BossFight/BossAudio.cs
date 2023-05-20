using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace New
{

    public class BossAudio : MonoBehaviour
    {

        [SerializeField] private List<AudioClip> _clips; // 0-4 -attacks, 5-6 stage, 7- teleport, 8-death
        [SerializeField] private AudioSource _externalAudio;
        private AudioSource _audio;

        private float _timer = 10f;
        private void Awake() {
            _audio = GetComponent<AudioSource>();
        }

        private void Update() {
            if (_timer <= 0){
                _audio.PlayOneShot(_clips[6]);
                _timer = Random.Range(10f, 30f);
            }
            _timer -= Time.deltaTime;
        }

        public void ProceedAttackSound(BossAttackType type){
            switch (type)
            {  
                case BossAttackType.FireBall:
                    _audio.clip = _clips[(int)type];
                    break;
                case BossAttackType.Spikes:
                    _audio.clip = _clips[(int)type];
                    break;
                case BossAttackType.RailGun:
                    _audio.clip = _clips[(int)type];
                    break;
                case BossAttackType.FloorIsLava:
                    _audio.clip = _clips[(int)type];
                    break;
                case BossAttackType.BulletHell:
                    _audio.PlayOneShot(_clips[(int)type -1]);
                    _audio.clip = _clips[(int)type];
                    break;
                default:
                return;
            }
            _audio.PlayDelayed(1f);
        }

        public void StageShiftAudio(){
            StandardPlay(5);
            _audio.clip = _clips[6];
            _audio.PlayDelayed(0.3f);
        }

        public void TeleportationAudio(){
            StandardPlay(7);
        }

        public void DeathSound(){
            _externalAudio.Stop();
            StandardPlay(8);
        }
        private void StandardPlay(int indexInClips){
           
            _audio.PlayOneShot(_clips[indexInClips]);
        }

    }

}
