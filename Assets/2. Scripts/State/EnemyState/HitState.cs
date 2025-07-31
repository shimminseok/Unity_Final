using System;
using UnityEngine;

namespace EnemyState
{
    public class HitState : IState<EnemyUnitController, EnemyUnitState>
    {
        private readonly int isHit = Define.HitAnimationHash;
        private bool canCounter;
        private bool hasHandler;

        public void OnEnter(EnemyUnitController owner)
        {
            owner.IsAnimationDone = false;
            owner.Animator.SetTrigger(isHit);
            owner.PlayHitVoiceSound();
            canCounter = owner.CanCounterAttack(owner.LastAttacker);
            hasHandler = false;
            if (owner.LastAttacker == null)
            {
                return;
            }

            Action onHitFinished = null;

            if (canCounter)
            {
                onHitFinished = () =>
                {
                    owner.StartCountAttack(owner.LastAttacker);
                };
            }
            else if (!owner.LastAttacker.SkillController.CurrentSkillData?.skillSo.skillTimeLine)
            {
                onHitFinished = owner.LastAttacker.InvokeHitFinished;
            }

            if (onHitFinished != null)
            {
                owner.OnHitFinished += onHitFinished;
            }
        }

        public void OnUpdate(EnemyUnitController owner)
        {
            if (hasHandler || !owner.IsAnimationDone)
            {
                return;
            }

            hasHandler = true;
        }

        public void OnFixedUpdate(EnemyUnitController owner)
        {
        }

        public void OnExit(EnemyUnitController owner)
        {
        }
    }
}