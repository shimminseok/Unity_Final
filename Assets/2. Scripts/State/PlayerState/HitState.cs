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
            Debug.Log("Enter Hit State");
            owner.IsAnimationDone = false;
            owner.Animator.SetTrigger(isHit);
            canCounter = owner.CanCounterAttack(owner.LastAttacker);
            hasHandler = false;
        }

        public void OnUpdate(PlayerUnitController owner)
        {
            if (hasHandler || !owner.IsAnimationDone)
            {
                return;
            }

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