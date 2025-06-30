using UnityEngine;

namespace PlayerState
{
    public class ReturnState : IState<PlayerUnitController, PlayerUnitState>
    {
        private readonly int isMove = Define.MoveAnimationHash;

        public void OnEnter(PlayerUnitController owner)
        {
            owner.Agent.speed = 10f;
            owner.Agent.avoidancePriority = 10;
            owner.setRemainDistance = 0.1f;
            owner.Animator.SetBool(isMove, true);
            owner.MoveTo(owner.StartPostion);
        }

        public void OnUpdate(PlayerUnitController owner)
        {
        }

        public void OnFixedUpdate(PlayerUnitController owner)
        {
        }

        public void OnExit(PlayerUnitController owner)
        {
            owner.Animator.SetBool(isMove, false);
            owner.transform.localRotation = Quaternion.identity;
        }
    }
}