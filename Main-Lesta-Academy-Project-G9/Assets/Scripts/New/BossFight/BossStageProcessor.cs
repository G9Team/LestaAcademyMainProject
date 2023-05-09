using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{

    public class BossStageProcessor : MonoBehaviour
    {
        [SerializeField] private BossPosition _bossPosotoin;
        [SerializeField] private Transform _positionToShiftStage;
        [SerializeField] private BossHealth _health;
        public BossStage Stage { get; private set; }

        private void Start()
        {
            Stage = BossStage.First;
        }

        public void ProceedNextStage()
        {
            _bossPosotoin.ChangePosition(_positionToShiftStage);
            //await for shift to position
            //PlayAnimation(); //Do we need this here? Boss Animation Controller?
            Stage++;
            Debug.Log("changing...");

        }
    }
}
