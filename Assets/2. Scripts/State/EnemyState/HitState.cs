using System;
using UnityEngine;

namespace EnemyState
{
    public class HitState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int isHit = Define.HitAnimationHash;
        private bool canCounter;
        private bool hasHandler;
        private Action onAttackFinishedHandler;

        private Coroutine counterCoroutine;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.IsAnimationDone = false;
            owner.Animator.SetTrigger(isHit);
            canCounter = owner.CanCounterAttack(owner.LastAttacker);
            hasHandler = false;
        }

        public void OnUpdate(EnemyUnitController owner)
        {
            if (hasHandler || !owner.IsAnimationDone)
            {
                return;
            }

            Debug.Log("Update Enemey Hit");
            hasHandler = true;
            if (canCounter)
            {
                owner.StartCountAttack(owner.LastAttacker);
            }
            else
            {
                owner.LastAttacker.InvokeHitFinished();
            }
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
            if (canCounter)
            {
                owner.OnMeleeAttackFinished -= onAttackFinishedHandler;
                onAttackFinishedHandler = null;
            }
        }
    }
}