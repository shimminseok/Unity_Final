using System;
using UnityEngine;

namespace PlayerState
{
    public class HitState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int isHit = Define.HitAnimationHash;
        private bool canCounter;
        private bool hasHandler;
        private Action onAttackFinishedHandler;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.IsAnimationDone = false;
            owner.Animator.SetTrigger(isHit);
            owner.PlayHitVoiceSound();
            Unit lastAttacker = owner.LastAttacker;
            canCounter = owner.CanCounterAttack(lastAttacker);
            hasHandler = false;
            if (lastAttacker == null)
            {
                return;
            }

            Action onHitFinished = null;

            if (canCounter)
            {
                onHitFinished = () =>
                {
                    owner.StartCountAttack(lastAttacker);
                };
            }
            else if (!lastAttacker.SkillController.CurrentSkillData?.skillSo.skillTimeLine)
            {
                onHitFinished = lastAttacker.InvokeHitFinished;
            }

            if (onHitFinished != null)
            {
                owner.OnHitFinished += onHitFinished;
            }
        }

        public void OnUpdate(PlayerUnitController owner)
        {
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController owner)
        {
            if (canCounter)
            {
                owner.OnMeleeAttackFinished -= onAttackFinishedHandler;
                onAttackFinishedHandler = null;
            }
        }
    }
}