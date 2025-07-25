using UnityEngine;

namespace PlayerState
{
    public class HitState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int isHit = Define.HitAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            Debug.Log("Enter Hit State");
            owner.Animator.SetTrigger(isHit);
        }

        public void OnUpdate(PlayerUnitController owner)
        {
            if (!owner.IsAnimationDone && owner.LastAttacker == null)
            {
                return;
            }

            if (owner.CanCounterAttack(owner.LastAttacker))
            {
                owner.StartCountAttack(owner.LastAttacker);
            }
            else
            {
                owner.InvokeHitFinished();
            }
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController owner)
        {
            owner.SetLastAttacker(null);
        }
    }
}