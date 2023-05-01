using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{

    public class BossOrchestrator : MonoBehaviour
    {
        [SerializeField] private GameObject _bossBody;
        [SerializeField] private float _loopTimer;
        [SerializeField] private BossHealth _health;
        private BossPosition _bossPosition;
        private BossStageProcessor _stageProcessor;
        private BossAttackHandler _attackHandler;
        private float _actualTimer;
        private List<BossAttackType> _firstStageAttacks = new List<BossAttackType> { BossAttackType.FireBall, BossAttackType.Spikes };
        private List<BossAttackType> _secondStageAttacks = new List<BossAttackType> { BossAttackType.RailGun };
        private List<BossAttackType> _thirdStageAttacks = new List<BossAttackType> { BossAttackType.BulletHell };
        private void Awake()
        {
            RefreshAttacksLists();
            _attackHandler = GetComponent<BossAttackHandler>();
            _bossPosition = GetComponent<BossPosition>();
            _stageProcessor = GetComponent<BossStageProcessor>();
            _actualTimer = _loopTimer;
            _health.Death += Die;
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
            CalculateAttack();


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
        private void OnDisable() {
            _health.Death -= Die;
        }

    }
}
