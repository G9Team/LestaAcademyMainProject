using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{

    public class BossOrchestrator : MonoBehaviour
    {
        [SerializeField] private GameObject _bossBody, _stageVFX;
        [SerializeField] private float _loopTimer, _breakTimer;
        [SerializeField] private BossHealth _health;
        private Animator _animator;
        private BossPosition _bossPosition;
        private BossStageProcessor _stageProcessor;
        private BossAttackHandler _attackHandler;
        private float _actualTimer, _actualBreak;
        private List<BossAttackType> _firstStageAttacks = new List<BossAttackType> { BossAttackType.FireBall, BossAttackType.Spikes };
        private List<BossAttackType> _secondStageAttacks = new List<BossAttackType> { BossAttackType.RailGun };
        private List<BossAttackType> _thirdStageAttacks = new List<BossAttackType> { BossAttackType.BulletHell };
        private int _attackCounter = 6;
        private bool _stopper = false;
        private void Awake()
        {
            RefreshAttacksLists();
            _attackHandler = GetComponent<BossAttackHandler>();
            _bossPosition = GetComponent<BossPosition>();
            _stageProcessor = GetComponent<BossStageProcessor>();
            _actualTimer = _loopTimer;
            _actualBreak = _breakTimer;
            _health.Death += Die;
            _health.Damaged += GetDamage;
            _health.NextStage += NextStage;
            _animator = _health.gameObject.GetComponent<Animator>();
        }
        private void RefreshAttacksLists()
        {
            _secondStageAttacks.AddRange(_firstStageAttacks);
            _thirdStageAttacks.AddRange(_secondStageAttacks);
        }
        private void Start() {
            NextLoop();
        }

        private void Update()
        {
            if (_stopper){
                
                _actualBreak -= Time.deltaTime;
                if(_actualBreak <= 0){
                    _actualBreak = _breakTimer;
                    _stopper = false;
                }
                return;
            }

            _actualTimer -= Time.deltaTime;
            if (_actualTimer <= 0)
            {

                NextLoop();
            }
        }

        private void NextLoop()
        {
            _actualTimer = _loopTimer;
            _bossPosition.ChangePosition();
            Debug.Log("NextLoop");
            CalculateAttack();


        }

        private void NextStage(){
            StopCoroutine(GetDamageTime());
            _stageProcessor.ProceedNextStage();
            _actualBreak = 2.5f;
            _stageVFX.SetActive(true);
            _stopper = true;
        }
        private void CalculateAttack()
        {
            switch (_stageProcessor.Stage)
            {
                case BossStage.First:
                    _attackHandler.ProceedAttack(_firstStageAttacks[Random.Range(0, _firstStageAttacks.Count)]);
                    break;
                case BossStage.Second:

                    _attackHandler.ProceedAttack(AttackWithPosition(_secondStageAttacks));

                    break;
                case BossStage.Third:
                    _attackHandler.ProceedAttack(AttackWithPosition(_thirdStageAttacks));
                    break;
            }
        }
        private BossAttackType AttackWithPosition(List<BossAttackType> attackTypes)
        {
            if (_bossPosition.LowSidePositions.Contains(_bossPosition.CurrentPosition) == false) { return attackTypes[Random.Range(0, attackTypes.Count)]; }
            if (_stageProcessor.Stage == BossStage.Second) { return BossAttackType.FloorIsLava; }
            List<BossAttackType> third = new List<BossAttackType>{BossAttackType.BulletHell, BossAttackType.FloorIsLava};
            return third[Random.Range(0, third.Count -1)];
        }
        private void Die()
        {
            _bossBody.GetComponent<Animator>().Play("death");
            Destroy(_bossBody, 2.5f);
            Destroy(this.gameObject);
            
        }
        private void GetDamage(){
            StartCoroutine(GetDamageTime());
            
        }
        private IEnumerator GetDamageTime(){
            _animator.Play("Damaged");
            yield return new WaitForSeconds(0.3f);
            _bossPosition.ChangePosition();
            _attackCounter--;
            if (_attackCounter <=0){
                CoffeeBreak();
                Debug.Log("Coffie");
            }
            Debug.Log("Damaged");

        }
        private void CoffeeBreak(){
            _bossPosition.ChangePositionToSafe();
            _attackCounter = 6;
            _stopper = true;
        }
        private void OnDisable() {
            _health.Death -= Die;
            _health.Damaged -= GetDamage;
        }

    }
}
