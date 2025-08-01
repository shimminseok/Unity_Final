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